using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood
{
    public class Greeter : IGreeter
    {
        private IConfiguration _configuration;

        public Greeter(IConfiguration config)
        {
            _configuration = config;
        }

        public string GetMessageOfTheDay()
        {
            return _configuration["Greeting"];
        }
    }
}
