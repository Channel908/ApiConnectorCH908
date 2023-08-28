using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiConnectorCH908;

internal static class HttpExtensionMethods
{

    internal static HttpResponseData CreateJsonResponse(this HttpRequestData req, object body, HttpStatusCode statusCode = HttpStatusCode.OK)
        => CreateResponse(req, JsonConvert.SerializeObject(body), statusCode);

    internal static HttpResponseData CreateBadRequestResponse(this HttpRequestData req, object body)
        => CreateResponse(req, JsonConvert.SerializeObject(body), HttpStatusCode.BadRequest);

    internal static HttpResponseData CreateErrorResponse(this HttpRequestData req, string? body = null)
    {
        body ??= "Internal Server Error";
        return CreateResponse(req, JsonConvert.SerializeObject(body), HttpStatusCode.InternalServerError);
    }

    internal static HttpResponseData CreateResponse(this HttpRequestData req, string body, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var ret = HttpResponseData.CreateResponse(req);
        ret.StatusCode = statusCode;
        ret.Headers.Add("Content-Type", "application/json; charset=utf-8");
        ret.WriteString(body);
        return ret;
    }



}
