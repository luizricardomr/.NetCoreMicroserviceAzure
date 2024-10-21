using Mango.Services.EmailApi.DTO;
using Mango.Services.EmailApi.Messages;

namespace Mango.Services.EmailApi.Services
{
	public interface IEmailService
	{
		Task EmailCartAndLog(CartDTO dto);
		Task RegisterUserEmailAndLog(string email);
		Task LogOrderPlaced(RewardsMessage message);
	}
}
