using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductProvider
    {
        public Task<(bool IsSuccess, IEnumerable<ProductDto> Products, string ErrorMessage)> GetProductsAsync();
        public Task<(bool IsSuccess, ProductDto Product, string ErrorMessage)> GetProductAsync(int id);
    }
}
