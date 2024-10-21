using Mango.Services.ShoppingCart.Models.DTO;

namespace Mango.Services.ShoppingCart.Services.IService
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCoupon(string couponCode);
    }
}
