using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System.Net;

namespace ApiConnectorCH908;

internal static class HttpExtensionMethods
{
    
    internal const string INTERNAL_SERVER_ERROR = "Internal Server Error";
    internal const string BAD_REQUEST = "Bad Request";
    internal const string BLOCK_MESSAGE = "Whoops, something went wrong";

    internal static HttpResponseData CreateJsonResponse(this HttpRequestData req, object body, HttpStatusCode statusCode = HttpStatusCode.OK)
        => CreateResponse(req, JsonConvert.SerializeObject(body), statusCode);

    internal static HttpResponseData CreateBadRequestResponse(this HttpRequestData req, string? body = null)
        => req.CreateBadRequestResponse(body ?? BAD_REQUEST);

    internal static HttpResponseData CreateErrorResponse(this HttpRequestData req, string? body = null)
        => req.CreateResponse(body ?? INTERNAL_SERVER_ERROR, HttpStatusCode.InternalServerError);

    internal static HttpResponseData CreateResponse(this HttpRequestData req, string body, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var ret = HttpResponseData.CreateResponse(req);
        ret.StatusCode = statusCode;
        ret.Headers.Add("Content-Type", "application/json; charset=utf-8");
        ret.WriteString(body);
        return ret;
    }

    internal static HttpResponseData CreateBlockResponse(this HttpRequestData req, string? userMessage = null)
        =>  req.CreateJsonResponse(
                new
                {
                    version = "1.0.0",
                    action = "ShowBlockPage",
                    userMessage = userMessage ?? BLOCK_MESSAGE
                },

                HttpStatusCode.BadRequest);

}
