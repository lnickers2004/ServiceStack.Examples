using System;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace ServiceStack.Northwind.ServiceInterface
{
    public class CustomerDetailsService : Service
    {
        public CustomerDetailsResponse Get(CustomerDetails request)
        {
            var customer = Db.GetByIdOrDefault<Customer>(request.Id);
            if (customer == null)
                throw new HttpError(HttpStatusCode.NotFound,
                                    new ArgumentException("Customer does not exist: " + request.Id));

            var ordersService = base.ResolveService<OrdersService>();
            var ordersResponse = (OrdersResponse) ordersService.Get(new Orders {CustomerId = customer.Id});

            return new CustomerDetailsResponse
            {
                Customer = customer,
                CustomerOrders = ordersResponse.Results,
            };
        }
    }
}
