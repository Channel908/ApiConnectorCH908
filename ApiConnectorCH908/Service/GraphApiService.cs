using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ApiConnectorCH908.Service;

public class GraphApiService : IDisposable
{
    private GraphServiceClient? _graphClient { get; set; } = null;

    private readonly string? _clientId;
    private readonly string? _tenantId;
    private readonly string? _principleId;
    private readonly string? _clientSecret;

    public GraphApiService()
    {
        _clientId = Environment.GetEnvironmentVariable("ClientId");
        _tenantId = Environment.GetEnvironmentVariable("TenantId");
        _principleId = Environment.GetEnvironmentVariable("PrincipleId");
        _clientSecret = Environment.GetEnvironmentVariable("ClientSecret");

        var clientSecretCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);

        _graphClient = new GraphServiceClient(clientSecretCredential);
    }


    public async Task<IEnumerable<string>?> GetUserRoles(string userId)
    {
        if (_graphClient is null || string.IsNullOrEmpty(userId))
            return Enumerable.Empty<string>();

        try
        {
            var userTask = _graphClient.Users[userId].AppRoleAssignments.GetAsync();

            var principalTask = _graphClient.ServicePrincipals[_principleId].GetAsync();

            await Task.WhenAll(userTask, principalTask);

            var user = userTask.Result;

            if (user?.Value is null || !user.Value.Any()) 
                return Enumerable.Empty<string>();

            var principal = principalTask.Result;

            if (principal?.AppRoles is null || !principal.AppRoles.Any()) 
                return Enumerable.Empty<string>();

            var roleIds = user.Value
                                .Select(x => x.AppRoleId.ToString())
                                .ToList();

            var result = principal.AppRoles
                                .Where(x => roleIds.Contains(x.Id.ToString()))
                                .Select(x => x.Value)
                                .ToList();

            result?.Add("User");

            return result!;
        }
        catch
        {
            return Enumerable.Empty<string>();

        }

    }


    public void Dispose()
    {
        _graphClient?.Dispose();
    }
}
