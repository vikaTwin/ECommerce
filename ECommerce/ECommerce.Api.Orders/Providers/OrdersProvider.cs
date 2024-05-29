using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext context;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext context, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!context.Orders.Any())
            {
                context.Orders.Add(new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow,
                    Total = 2,
                    OrderItems = new List<OrderItem>()
                    {
                        new() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 3, UnitPrice = 5 },
                        new() { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, UnitPrice = 15 }
                    },
                });

                context.Orders.Add(new Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    Total = 100,
                    OrderItems = new List<OrderItem>()
                    {
                        new() { Id = 3, OrderId = 2, ProductId = 3, Quantity = 13, UnitPrice = 55 }
                    },
                });

                context.Orders.Add(new Order
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.UtcNow.AddMinutes(30),
                    Total = 66,
                    OrderItems = new List<OrderItem>()
                    {
                        new() { Id = 4, OrderId = 3, ProductId = 1, Quantity = 4, UnitPrice = 33 },
                        new() { Id = 5, OrderId = 3, ProductId = 2, Quantity = 6, UnitPrice = 44 },
                        new() { Id = 6, OrderId = 3, ProductId = 2, Quantity = 7, UnitPrice = 77 }
                    },
                });

                context.SaveChanges();
            }    
        }

        public async Task<(bool IsSuccess, IEnumerable<OrderDto> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.OrderItems)
                    .ToListAsync();

                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<OrderDto>>(orders);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
