using ApiConnectorCH908.Models;
using ApiConnectorCH908.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiConnectorCH908.Functions;

public class Policy
{
    private readonly ILogger _logger;
    private readonly GraphApiService _graphApi;

    public Policy(ILoggerFactory loggerFactory, GraphApiService graphApi)
    {
        _logger = loggerFactory.CreateLogger<Policy>();
        _graphApi = graphApi;
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

    [Function("GetUserRoles")]
    public async Task<HttpResponseData> GetUserRoles([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var requestorInfo = JsonConvert.DeserializeObject<RequestorInfo>(body);

        var roleList = await _graphApi.GetUserRoles(requestorInfo.objectId);

        var roles = new
        {
            extension_Roles = string.Join(",", roleList)
        };

        return req.CreateJsonResponse(roles);
    }
}




