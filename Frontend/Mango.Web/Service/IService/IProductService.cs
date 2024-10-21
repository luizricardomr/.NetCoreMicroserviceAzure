using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO> GetAllProductsAsync();
        Task<ResponseDTO> GetProductByIdAsync(int id);
        Task<ResponseDTO> CreateProductAsync(ProductDTO dto);
        Task<ResponseDTO> UpdateProductAsync(ProductDTO dto);
        Task<ResponseDTO> DeleteProductAsync(int id);
    }
}
