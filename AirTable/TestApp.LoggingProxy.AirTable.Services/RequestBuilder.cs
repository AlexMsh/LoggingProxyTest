using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.LoggingProxy.AirTable.Services
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly string _baseUrl;
        private readonly string _applicationKey;


        public RequestBuilder(string baseUrl, string applicationKey)
        {
            _baseUrl = baseUrl;
            _applicationKey = applicationKey;
        }

        public (RestClient, RestRequest) BuildRequest(Method method, string url, object body = null)
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = new JwtAuthenticator(_applicationKey);

            var request = new RestRequest(url, method);
            request.JsonSerializer = new RestSharpSerializer();

            if (body != default)
            {
                request.AddJsonBody(body);
            }

            return (client, request);
        }
    }
}
