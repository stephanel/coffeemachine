using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class OrangeCommand : BeverageCommand
    {
        public OrangeCommand()
        {
            this.Price = .6m;
        }
    }
}
