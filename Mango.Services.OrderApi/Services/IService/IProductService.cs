using Mango.Services.OrderApi.Models.DTO;

namespace Mango.Services.OrderApi.Services.IService
{
	public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
