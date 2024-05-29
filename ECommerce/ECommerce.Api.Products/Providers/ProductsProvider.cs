using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductProvider
    {
        private readonly ILogger<ProductsProvider> Logger;
        private readonly IMapper Mapper;
        private readonly ProductsDbContext DbContext;
        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!DbContext.Products.Any())
            {
                DbContext.Products.Add(new Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                DbContext.Products.Add(new Product() { Id = 2, Name = "Mouse", Price = 10, Inventory = 220 });
                DbContext.Products.Add(new Product() { Id = 3, Name = "Monitor", Price = 140, Inventory = 60 });
                DbContext.Products.Add(new Product() { Id = 4, Name = "CPU", Price = 270, Inventory = 88 });
                DbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductDto> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await DbContext.Products.ToListAsync();
                if (products != null && products.Any())
                {
                    var result = Mapper.Map<IEnumerable<ProductDto>>(products);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ProductDto Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = Mapper.Map<ProductDto>(product);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
