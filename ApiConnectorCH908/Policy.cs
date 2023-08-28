using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ApiConnectorCH908;

public class Policy
{
    private readonly ILogger _logger;

    public Policy(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Policy>();
    }

    [Function("PolicyAdmin")]
    public HttpResponseData PolicyAdmin([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        var roles = new
        {
            extension_Roles = "Admin"
        };

        return req.CreateJsonResponse(roles);
    }
}
