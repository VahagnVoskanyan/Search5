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

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsProduction()) // in K8S
            {
                Console.WriteLine("--> Using SqlServer Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("CustomersConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }
            


            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //For Dtos
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>(); // Singleton => for Message Bus
            builder.Services.AddHostedService<MessageBusSubscriber>();                //Subscribe from bus
            builder.Services.AddGrpc();

            var app = builder.Build();


            PrepDb.PrepPopulation(app); //For Mock data

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting(); // 

            app.UseAuthorization();

            app.MapControllers();

            //app.MapGrpcService<GrpcCustomerService>(); //

            app.MapGet("/protos/customers.proto", async context =>
            {
                await context.Response.WriteAsync(File.ReadAllText("Protos/customers.proto"));
            }); //It's not not necessary, we give info about proto file

            app.Run();
        }
    }
}