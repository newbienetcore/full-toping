using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Users.Models;
using Notifications.Domain.Entities;

namespace Notifications.Application.Users.Commands.SignUp;

public record SignUpCommand : IRequest<AuthenticationResult>
{
    public string UserName { get; set; }
    public string Code { get; set; }
    public string CountryCode { get; set; }
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthenticationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SignUpCommandHandler(IApplicationDbContext context, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }
    

    public async Task<AuthenticationResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        
        if (userExist is not null)
            throw new BadRequestException("User with given UserName already exists.");
        
        var user = new User()
        {
            UserName = request.UserName,
            Code = request.Code,
            CountryCode = request.CountryCode
        };
        
        await _context.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
        await _userManager.AddToRoleAsync(user, "Admin");
        await _context.SaveChangesAsync(cancellationToken);

        // var user = await _userRepository.CreateAsync(userNew, cancellationToken);
        
        var token = await _jwtTokenGenerator.Generate(user);

        return new AuthenticationResult(user, token);
    }
}
