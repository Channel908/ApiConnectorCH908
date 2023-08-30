using ApiConnectorCH908.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph.Models.ExternalConnectors;
using System.Xml.Linq;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()

    .ConfigureServices(services => {
        services.AddOptions<GraphApiServiceOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AzureAD").Bind(settings);
                });

        services.AddScoped<GraphApiService>();
     })

    .Build();

host.Run();
