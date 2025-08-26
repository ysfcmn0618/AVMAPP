using AVMAPP.Services.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Services
{
    public static class ServiceExtensionsMvc
    {
        public static void ConfigureMvcServices(this IServiceCollection services, IConfiguration configuration)
        {
            // API ile haberleşmek için HttpClient
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
            });

            // AutoMapper (MVC tarafında ViewModel ↔ DTO eşlemeleri için)
            services.AddAutoMapper(typeof(OrderItemProfile).Assembly);
        }
    }
}
