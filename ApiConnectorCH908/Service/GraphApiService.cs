using Azure.Identity;
using Microsoft.Extensions.Options;
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

    public GraphApiServiceOptions GraphApiServiceOptions { get; internal set; }

    public GraphApiService(IOptions<GraphApiServiceOptions> options)
    {

        GraphApiServiceOptions = options.Value;


        _graphClient = new GraphServiceClient
        ( 
            new ClientSecretCredential(options.Value.TenantId, options.Value.ClientId, options.Value.ClientSecret)
        );
    }


    public async Task<IEnumerable<string>?> GetUserRoles(string userId)
    {
        if (_graphClient is null || string.IsNullOrEmpty(userId))
            return Enumerable.Empty<string>();

        try
        {
            var userTask = _graphClient.Users[userId].AppRoleAssignments.GetAsync();

            var principalTask = _graphClient.ServicePrincipals[GraphApiServiceOptions.PrincipleId].GetAsync();

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
            result?.Add($"RoleTest {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            return result!;
        }
        catch(Exception ex)
        {
            return Enumerable.Empty<string>();

        }

    }


    public void Dispose()
    {
        _graphClient?.Dispose();
    }
}
