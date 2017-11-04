using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class CoffeeCommand : HotBeverageCommand
    {
        public CoffeeCommand(int sugarCount, bool includeTouillette, bool extraHot)
            : base(sugarCount, includeTouillette, extraHot)
        {
            this.Price = .6m;
        }
    }
}
