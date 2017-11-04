using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeMaker.Interfaces;
using CoffeeMaker.Models;

namespace CoffeeMaker.Services
{
    public class Repository : IRepository
    {
        readonly List<ReportCommand> _datas;
        public Repository()
        {
            this._datas = new List<ReportCommand>();
        }

        public void AddCommand(BeverageCommand command, DateTime when)
        {
            this._datas.Add(new ReportCommand (command, when ));
        }

        public List<ReportCommand> GetReport()
        {
            return _datas;
        }

    }
}
