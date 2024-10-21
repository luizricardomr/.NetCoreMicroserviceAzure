using Mango.Services.Reward.Api.Messages;

namespace Mango.Services.RewardsApi.Services
{
    public interface IRewardsService
	{
		Task UpdateReward(RewardsMessage Message);
	}
}
