using Mango.Services.Reward.Api.Messages;
using Mango.Services.Reward.Api.Models;
using Mango.Services.RewardApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.RewardsApi.Services
{
    public class RewardsService : IRewardsService
	{
		private DbContextOptions<AppDbContext> _dbOptions;

		public RewardsService(DbContextOptions<AppDbContext> dbOptions)
		{
			this._dbOptions = dbOptions;
		}

        public async Task UpdateReward(RewardsMessage message)
        {
			try
			{
				Rewards rewards = new()
				{
					OrderId = message.OrderId,
					RewardsActivity = message.RewardsActivity,
					UserId = message.UserId,
					RewardsDate = DateTime.Now
				};
				await using var _db = new AppDbContext(_dbOptions);
				await _db.Rewards.AddAsync(rewards);
				await _db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
			}
		}
	}
}
