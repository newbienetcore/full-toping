using OnlineShop.Core.Models;
namespace OnlineShop.Core.Interfaces
{
    public interface IMailService
    {
        Task SendMail(MailContentModel mailContent);
    } 
}
