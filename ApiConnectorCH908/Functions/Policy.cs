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
    public HttpResponseData PolicyAdmin([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
    {
 
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

            var body = await new StreamReader(req.Body).ReadToEndAsync();

            var requestorInfo = JsonConvert.DeserializeObject<RequestorInfo>(body);

            if (!requestorInfo.Validate(_graphApi.GraphApiServiceOptions.ClientId))
            {
                _logger.LogError("Invalid request data");
                return req.CreateBadRequestResponse();
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




