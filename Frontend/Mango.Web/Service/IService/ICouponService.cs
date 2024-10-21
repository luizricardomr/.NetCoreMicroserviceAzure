using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO> GetCouponAsync(string couponCode);
        Task<ResponseDTO> GetAllCouponsAsync();
        Task<ResponseDTO> GetCouponByIdAsync(int id);
        Task<ResponseDTO> CreateCouponAsync(CouponDTO dto);
        Task<ResponseDTO> UpdateCouponAsync(CouponDTO dto);
        Task<ResponseDTO> DeleteCouponAsync(int id);

    }
}
