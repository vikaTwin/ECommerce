using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Profiles
{
    public class CustomersProfile : AutoMapper.Profile
    {
        public CustomersProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
