using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker.Interfaces
{
    public interface IEmailNotifier
    {
        void NotifyMissingDrink(String drink);
    }        
}
