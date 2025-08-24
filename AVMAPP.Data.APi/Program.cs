using AVMAPP.Data.APi.Models;
using AVMAPP.Services;
using AVMAPP.Services.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Automapper configuration
builder.Services.AddAutoMapper(typeof(OrderItemProfile).Assembly);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
