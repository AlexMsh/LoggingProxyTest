using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.LoggingProxy.AirTable.Services
{
    public interface IRequestBuilder
    {
        (RestClient, RestRequest) BuildRequest(Method method, string url, object body = null);
    }
}
