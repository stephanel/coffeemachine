using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class HotBeverageCommand : BeverageCommand
    {
        public bool ExtraHot { get; protected set; }

        public HotBeverageCommand(int sugarCount, bool includeTouillette, bool extraHot)
            : base(sugarCount, includeTouillette)
        {
            this.ExtraHot = extraHot;
        }

    }
}
