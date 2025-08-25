using AVMAPP.Data.Infrastructure;
using AVMAPP.Data.Infrastructure.AVMDbContext;
using AVMAPP.Services.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AVMAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));    
            services.AddAutoMapper(typeof(OrderItemProfile).Assembly);
        }
        
    }
}
