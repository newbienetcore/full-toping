using Microsoft.Extensions.Configuration;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.UseCases.Mail.SendMail
{
	public class SendMailFlow
	{
		private readonly IMailService _mailService;

		public SendMailFlow(IMailService mailService) 
		{
			_mailService = mailService;
		}
		public async Task SendMail(MailContentModel mailContent)
		{
			await _mailService.SendMail(mailContent);
		}
	}
}
