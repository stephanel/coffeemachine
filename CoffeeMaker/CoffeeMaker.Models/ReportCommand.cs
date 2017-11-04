using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class ReportCommand
    {
        public BeverageCommand Command { get; private set; }
        public DateTime When { get; private set; }

        public ReportCommand(BeverageCommand command, DateTime when)
        {
            Command = command;
            When = when;
        }
    }
}
