using AVMAPP.Data.Infrastructure.AVMDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AVMAPP.Data.Infrastructure.Factories
{
    public class AVMAppDbContextFactory : IDesignTimeDbContextFactory<AVMAppDbContext>
    {
        AVMAppDbContext IDesignTimeDbContextFactory<AVMAppDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AVMAppDbContext>();

            // ✅ Buraya kendi connection string'ini yaz
            optionsBuilder.UseSqlServer("Server=YSFCMN-HOME\\SQLEXPRESS;Database=AVMAppSMadeDb;Trusted_Connection=True;TrustServerCertificate=Yes");
            //var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=AVMAppDb;Trusted_Connection=True;";
            //optionsBuilder.UseSqlServer(connectionString);

            return new AVMAppDbContext(optionsBuilder.Options);
        }
    }
}



