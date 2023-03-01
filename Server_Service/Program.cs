using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Server_Service.AsyncDataServices;
using Server_Service.Data;
using Microsoft.Extensions.Hosting.WindowsServices;
using Server_Service.Controllers;
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
            //builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>(); //Singlton?
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>(); // Singlton => for Message Bus
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddHostedService<MessageBusSubscriber>();                //Subscribe from bus
            //builder.WebHost.UseUrls("http://localhost:5055", "https://localhost:7108");
            builder.Host.UseWindowsService(); // To work as Windows service
            builder.Host.UseSerilog();
            builder.Services.AddGrpc();

            var app = builder.Build();


            PrepDb.PrepPopulation(app); //For data example
            //Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsost", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(@"C:\Users\vahag\Desktop\Vahagn\Practise\Search5\LogFile.txt")
                .CreateLogger();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGrpcService<GrpcCustomerService>(); //
            app.MapGet("/protos/customers.proto", async context =>
            {
                await context.Response.WriteAsync(File.ReadAllText("Protos/customers.proto"));
            }); //It's not necessary, we give info abour proto file

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            //Checking for service run
            try
            {
                Log.Information("Starting up the service");
                app.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem staring the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}