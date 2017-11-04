using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class TeaCommand : HotBeverageCommand
    {
        public TeaCommand(int sugarCount, bool includeTouillette, bool extraHot)
            : base(sugarCount, includeTouillette, extraHot)
        {
            this.Price = .4m;
        }
    }
}
