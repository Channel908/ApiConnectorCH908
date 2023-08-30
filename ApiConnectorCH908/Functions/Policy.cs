using ApiConnectorCH908.Models;
using ApiConnectorCH908.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

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
    public async Task<HttpResponseData> PolicyAdmin([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
    {
        var requestorInfo = await req.GetRequestorInfoAsync();

        var roles = new
        {
            extension_Roles = "Admin"
        };

        return req.CreateJsonResponse(roles);
    }

    [Function("GetUserRoles")]
    public async Task<HttpResponseData> GetUserRoles([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
    {
        try
        {
            var requestorInfo = await req.GetRequestorInfoAsync();

            if (!requestorInfo.Validate(_graphApi.GraphApiServiceOptions.ClientId))
            {
                _logger.LogError("Invalid request data");
                return req.CreateBlockResponse();
            }

            var roleList = await _graphApi.GetUserRoles(requestorInfo!.objectId);

            var rolesToAdd = roleList?.Any() ?? false
                ? string.Join(",", roleList)
                : "User";

            var roles = new
            {
                extension_Roles = rolesToAdd
            };

            return req.CreateJsonResponse(roles);
        }
        catch (Exception ex)
        {
           return req.CreateErrorResponse("Not Working");
        }
    }
}




