using Microsoft.EntityFrameworkCore;
using Server_Service.AsyncDataServices;
using Server_Service.Data;
using Server_Service.SyncDataServices.gRPC;
using Search_Service.EventProcessing;

namespace Server_Service
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

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //For Dtos
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>(); // Singlton => for Message Bus
            builder.Services.AddHostedService<MessageBusSubscriber>();                //Subscribe from bus
            builder.Services.AddGrpc();

            var app = builder.Build();


            PrepDb.PrepPopulation(app); //For data example


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting(); // 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcCustomerService>(); //

                endpoints.MapGet("/protos/customers.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/customers.proto"));
                }); //It's not necessary, we give info about proto file
            });

            app.Run();
        }
    }
}