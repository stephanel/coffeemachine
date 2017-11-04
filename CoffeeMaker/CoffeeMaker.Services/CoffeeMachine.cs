using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeMaker.Interfaces;
using CoffeeMaker.Models;

namespace CoffeeMaker.Services
{
    public class CoffeeMachine : ICoffeeMachine
    {
        readonly IBeverageQuantityChecker _beverageQuantityChecker;
        readonly IEmailNotifier _emailNotifier;
        readonly IRepository _repository;

        public CoffeeMachine(IRepository repository, IBeverageQuantityChecker beverageQuantityChecker, IEmailNotifier emailNotifier)
        {
            _beverageQuantityChecker = beverageQuantityChecker;
            _emailNotifier = emailNotifier;
            _repository = repository;
        }

        bool IsThereShortage(string drink)
        {
            if(this._beverageQuantityChecker.IsEmpty(drink))
            {
                this._emailNotifier.NotifyMissingDrink(drink);
                return true;
            }
            return false;
        }

        public string Instruct(Command order, decimal amount)
        {
            string instruction = "";

            if (order.GetType() == typeof(MessageCommand))
            {
                var command = (MessageCommand)order;
                instruction = string.Format("M:{0}", command.Message);
            }
            else
            {
                var command = (BeverageCommand)order;

                if ((order as HotBeverageCommand) != null)
                {
                    // "beverage" instruction
                    if ((command as ChocolateCommand) != null)
                    {
                        if(this.IsThereShortage("H"))
                            return "M:There's a shortage with 'Chocolate'. A notification was sent to our customer service.";
                        instruction += "H:";
                    }
                    else if ((command as CoffeeCommand) != null)
                    {
                        if(this.IsThereShortage("C"))
                            return "M:There's a shortage with 'Coffee'. A notification was sent to our customer service.";
                        instruction += "C:";
                    }
                    else if ((command as TeaCommand) != null)
                    {
                        if(this.IsThereShortage("T"))
                            return "M:There's a shortage with 'Tea'. A notification was sent to our customer service.";
                        instruction += "T:";
                    }

                    if ((order as HotBeverageCommand).ExtraHot)
                    {
                        instruction = instruction.Replace(":", "h:");
                    }

                    // "sugar count" instruction
                    if (command.SugarCount == 0)
                        instruction += ":";
                    else
                        instruction += command.SugarCount.ToString() + ":";

                    // "touillette" instruction
                    if (command.SugarCount > 0)
                        instruction += "0";

                }
                else
                {
                    // "beverage" instruction
                    if (command.GetType() == typeof(OrangeCommand))
                    {
                        if (this.IsThereShortage("O"))
                            return "M:There's a shortage with 'Orange'. A notification was sent to our customer service.";
                        instruction += "O::";
                    }
                }

                // check price
                if (amount < command.Price)
                    instruction = string.Format("M:{0} euro is missing", command.Price - amount);

                // add command to repository
                this._repository.AddCommand(command, DateTime.Now);
            }

            return instruction;
        }

    }
}
