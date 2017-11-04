using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class ChocolateCommand : HotBeverageCommand
    {
        public ChocolateCommand(int sugarCount, bool includeTouillette, bool extraHot)
            : base(sugarCount, includeTouillette, extraHot)
        {
            this.Price = .5m;
        }
    }
}
