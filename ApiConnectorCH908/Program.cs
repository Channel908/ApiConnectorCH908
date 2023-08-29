using ApiConnectorCH908;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services => {
          services.AddScoped<GraphApi>();
     })

    .Build();

host.Run();
