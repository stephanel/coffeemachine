using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class MessageCommand : Command
    {
        public string Message { get; private set; }

        public MessageCommand(string message)
        {
            Message = message;
        }
    }
}
