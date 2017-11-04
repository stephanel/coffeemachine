using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Models
{
    public class BeverageCommand : Command
    {
        public decimal Price { get; protected set; }
        public int SugarCount { get; private set; }
        public bool IncludeTouillette { get; private set; }

        public BeverageCommand()
        { }

        public BeverageCommand(int sugarCount, bool includeTouillette)
        {
            SugarCount = sugarCount;
            IncludeTouillette = includeTouillette;
        }
    }
}
