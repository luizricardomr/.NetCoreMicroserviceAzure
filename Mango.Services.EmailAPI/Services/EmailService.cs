using Mango.Services.EmailApi.Data;
using Mango.Services.EmailApi.DTO;
using Mango.Services.EmailApi.Messages;
using Mango.Services.EmailApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailApi.Services
{
	public class EmailService : IEmailService
	{
		private DbContextOptions<AppDbContext> _dbOptions;

		public EmailService(DbContextOptions<AppDbContext> dbOptions)
		{
			this._dbOptions = dbOptions;
		}

		public async Task EmailCartAndLog(CartDTO dto)
		{
			StringBuilder message = new StringBuilder();

			message.AppendLine("<br/>Cart Email Requested ");
			message.AppendLine("<br/>Total " + dto.CartHeader.CartTotal);
			message.Append("<br/>");
			message.Append("<ul>");

			foreach (var item in dto.CartDetails)
			{
				message.Append("<li>");
				message.Append(item.Product.Name + " x " + item.Count);
				message.Append("</li>");
			}
			message.Append("</ul>");

			await LogAndEmail(message.ToString(), dto.CartHeader.Email);
		}

        public async Task LogOrderPlaced(RewardsMessage messageDTO)
        {
            string message = "New Order Placed. <br /> Order Id: " + messageDTO.OrderId;
			await LogAndEmail(message, "ricardo@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
		{
			string message = "User Registered Successfully. <br/> Email : " + email;
			await LogAndEmail(message, "ricardo@gmail.com");
		}

		private async Task<bool> LogAndEmail(string message, string email)
		{
			try
			{
				EmailLogger emailLog = new()
				{
					Email = email,
					EmailSent = DateTime.Now,
					Message = message
				};
				await using var _db = new AppDbContext(_dbOptions);
				await _db.EmailLoggers.AddAsync(emailLog);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
