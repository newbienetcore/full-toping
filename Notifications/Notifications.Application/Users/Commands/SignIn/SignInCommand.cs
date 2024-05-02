using MediatR;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Users.Models;
using Notifications.Domain.Entities;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.Application.Users.Commands.SignIn;

public class SignInCommand : IRequest<AuthenticationResult>
{
    public string UserName { get; set; }
    public string Code { get; set; }
    
}

public class SignUpCommandHandler : IRequestHandler<SignInCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignUpCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
    }
    
    public async Task<AuthenticationResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        
        if (user is  null)
            throw new ApplicationException(ErrorCode.UserNotFound, ErrorCode.UserNotFound);
        
        if(user.Code != request.Code)
            throw new ApplicationException(ErrorCode.UserNotFound, ErrorCode.UserNotFound);
        
        var token = await _jwtTokenGenerator.Generate(user);

        return new AuthenticationResult(user, token);
    }
}
