using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreAnatomySamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.ConfigureKestrel(serverOptions =>
                    //{
                    //    serverOptions.Listen(IPAddress.Any, 10000, listenOptions =>
                    //    {
                    //        //listenOptions.UseConnectionHandler<DoNothingHandler>();

                    //        listenOptions.Use(next => async context =>
                    //        {
                    //            Console.WriteLine(context.RemoteEndPoint.ToString());

                    //            await next(context);

                    //            //throw new InvalidOperationException();
                    //        });
                    //    });
                    //});

                    webBuilder.UseStartup<Startup>();
                });

        public class DoNothingHandler : ConnectionHandler
        {
            private readonly ILogger<DoNothingHandler> _logger;

            public DoNothingHandler(ILogger<DoNothingHandler> logger)
            {
                _logger = logger;
            }

            public override async Task OnConnectedAsync(ConnectionContext connection)
            {
                _logger.LogInformation(connection.ConnectionId + " connected");

                // Handle the connection

                _logger.LogInformation(connection.ConnectionId + " disconnected");
            }
        }
    }
}
