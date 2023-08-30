using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ApiConnectorCH908.Functions;

public class ReverseProxy
{
    private readonly ILogger _logger;

    public ReverseProxy(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ReverseProxy>();
    }

    [Function("RP")]
    public async Task<HttpResponseData> RP([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
    {

        string code = req.Query["code"]!;

        string name = req.Query["name"]!;

        if (string.IsNullOrEmpty(name))
            return req.CreateBadRequestResponse("Name parameter not supplied");

        var host = Environment.GetEnvironmentVariable("ProxyHost");

        if (string.IsNullOrEmpty(host))
            return req.CreateErrorResponse();

        try
        {

            using var cli = new HttpClient();

            cli.BaseAddress = new Uri($"http://{host}/api/");

            var message = new HttpRequestMessage(new HttpMethod(req.Method), $"{name}?code={code}");

            if (req.Method == HttpMethod.Post.Method)
                message.Content = new StreamContent(req.Body);

            var response = await cli.SendAsync(message);

            var responseBody = await response.Content.ReadAsStringAsync();

            return req.CreateResponse(responseBody);

        }
        catch (Exception ex)
        {
            return req.CreateErrorResponse(ex.Message);
        }

    }


}
