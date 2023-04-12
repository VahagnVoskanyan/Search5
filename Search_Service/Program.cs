using Microsoft.EntityFrameworkCore;
using Search_Service.AsyncDataServices;
using Search_Service.Data;
using Search_Service.SyncDataServices.gRPC;

namespace Search_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemS"));
            builder.Services.AddScoped<ICustomerRepoS, CustomerRepoS>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>(); //Client
            builder.Services.AddScoped<IGrpcDataClient, GrpcDataClient>();


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
        }
    }
}