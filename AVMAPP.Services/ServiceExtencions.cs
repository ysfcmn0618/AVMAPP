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
            // Replace 'AllProfile' with the correct AutoMapper profile class name.
            // If you have a profile class (e.g., 'MappingProfile'), use its type instead.
            // Example: services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // If you do not have any profile class, you need to create one in AVMAPP.Services.Profiles namespace.
            // For now, replace 'AllProfile' with the correct profile class name or ask for clarification if you are unsure.

            services.AddAutoMapper(typeof(OrderItemProfile).Assembly);
      
        }
        
    }
}
