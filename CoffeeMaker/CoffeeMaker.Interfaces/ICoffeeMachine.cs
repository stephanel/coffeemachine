using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeMaker.Models;

namespace CoffeeMaker.Interfaces
{
    public interface ICoffeeMachine
    {
        string Instruct(Command order, decimal amount);
    }
}
