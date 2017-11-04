using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeMaker.Models;

namespace CoffeeMaker.Interfaces
{
    public interface IRepository
    {
        void AddCommand(BeverageCommand command, DateTime when);

        List<ReportCommand> GetReport();
    }
}
