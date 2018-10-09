using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood
{
    public class Greeter : IGreeter
    {
        private IConfiguration _configuration; // steg 1.
        
        // Använder här dependency injuction pattern, vilket innebär att kontruktorn tar in en interface som inparameter och i kontruktorn tilldelar  input variabeln med en local varjabel med samma interface.
        public Greeter(IConfiguration config) // steg 2.
        {
            _configuration = config; // steg 3.
        }
        
        public string GetMessageOfTheDay()
        {
            
            return _configuration["Greeting"]; // steg 4  då man anropar locala variabeln för att den ska göra nånting dvs _configuration
        }
        
    }
}
