using ApiConnectorCH908.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services => {
          services.AddScoped<GraphApiService>();
     })

    .Build();

host.Run();
