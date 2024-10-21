using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO> GetCartByUserIdAsync(string userId);
        Task<ResponseDTO> UpSertCartAsync(CartDTO dto);
        Task<ResponseDTO> RemoveFromAsync(int cartDetailsId);
        Task<ResponseDTO> ApplyCouponAsync(CartDTO dto);
        Task<ResponseDTO> EmailCart(CartDTO dto);

    }
}
