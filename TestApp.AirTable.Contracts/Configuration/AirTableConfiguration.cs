using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.LoggingProxy.Contracts.Configuration.AirTable
{
    public class AirTableConfiguration
    {
        public string LoggingConsumerBaseUrl { get; set; }

        public string LoggingConsumerApiKey { get; set; }
    }
}
