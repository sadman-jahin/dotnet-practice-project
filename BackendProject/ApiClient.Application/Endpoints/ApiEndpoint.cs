using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Application.Endpoints
{
    public static class ApiEndpoint
    {
        private static string? _productServiceBaseUrl;
        private static string? _orderServiceBaseUrl;

        public static void Initialize(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _productServiceBaseUrl = configuration["ProductService:BaseUrl"]
                ?? throw new ArgumentNullException("ProductService:BaseUrl");

            _orderServiceBaseUrl = configuration["OrderService:BaseUrl"]
                ?? throw new ArgumentNullException("OrderService:BaseUrl");
        }

        // Product endpoints
        public static string ProductExists => _productServiceBaseUrl + "/api/v1/product/exists";
        public static string ProductDeduct => _productServiceBaseUrl + "/api/v1/product/deduct";
        public static string ProductById(long productId) => $"{_productServiceBaseUrl}/api/v1/product/{productId}";


        // Order endpoints
        public static string PendingOrders => _orderServiceBaseUrl + "/api/v1/order/pending";
        public static string CloseOrder(long orderId) => $"{_orderServiceBaseUrl}/api/v1/order/{orderId}/close";
    }

}
