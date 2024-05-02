using System.Net.Cache;
using Contracts.Services;
using Shared.Services.Email;

namespace Infrastructure.Services;

public class SendGridEmailService : IEmailService<MailRequest>
{
    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}