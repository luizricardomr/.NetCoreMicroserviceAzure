using Mango.Services.ShoppingCart.Models.DTO;

namespace Mango.Services.ShoppingCart.Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
