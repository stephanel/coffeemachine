using System;
using System.Collections.Generic;
using Xunit;
using CoffeeMaker.Interfaces;
using CoffeeMaker.Models;
using CoffeeMaker.Services;
using Moq;

namespace CoffeeMaker.Services.Tests
{
    public class CoffeeMachineShould
    {
        readonly Mock<IBeverageQuantityChecker> _mockedBeverageQuantityChecker;
        readonly Mock<IEmailNotifier> _mockedEmailNotifier;
        readonly IRepository _repository;
        readonly ICoffeeMachine _coffeeMachine;
        public CoffeeMachineShould()
        {
            _mockedBeverageQuantityChecker = new Mock<IBeverageQuantityChecker>();
            _mockedEmailNotifier = new Mock<IEmailNotifier>();
            _repository = new Repository();
            _coffeeMachine = new CoffeeMachine(_repository, _mockedBeverageQuantityChecker.Object, _mockedEmailNotifier.Object);
        }

        [Fact]
        public void Convert_Message_Command_To_Instruction()
        {
            // Arrange
            string message = "Hello!";
            string expected = "M:Hello!";

            MessageCommand command = new MessageCommand(message);

            // Act
            string actual = _coffeeMachine.Instruct(command, 0);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        //  Tea
        [InlineData("T", 0, 0, false, "M:0,4 euro is missing")]
        [InlineData("T", 0, 0.4, false, "T::")]
        [InlineData("T", 0, 1, false, "T::")]
        [InlineData("T", 0, 0.4, false, "T::")]
        [InlineData("T", 1, 0.4, false, "T:1:0")]
        [InlineData("T", 2, 0.4, false, "T:2:0")]
        [InlineData("T", 0, 0.4, true, "Th::")]
        [InlineData("T", 1, 0.4, true, "Th:1:0")]
        [InlineData("T", 2, 0.4, true, "Th:2:0")]
        //  Coffee
        [InlineData("C", 0, 0, false, "M:0,6 euro is missing")]
        [InlineData("C", 0, 0.6, false, "C::")]
        [InlineData("C", 0, 1, false, "C::")]
        [InlineData("C", 0, 0.6, false, "C::")]
        [InlineData("C", 1, 0.6, false, "C:1:0")]
        [InlineData("C", 2, 0.6, false, "C:2:0")]
        [InlineData("C", 0, 0.6, true, "Ch::")]
        [InlineData("C", 1, 0.6, true, "Ch:1:0")]
        [InlineData("C", 2, 0.6, true, "Ch:2:0")]
        //  Chocolate
        [InlineData("H", 0, 0, false, "M:0,5 euro is missing")]
        [InlineData("H", 0, 0.5, false, "H::")]
        [InlineData("H", 0, 1, false, "H::")]
        [InlineData("H", 0, 0.5, false, "H::")]
        [InlineData("H", 1, 0.5, false, "H:1:0")]
        [InlineData("H", 2, 0.5, false, "H:2:0")]
        [InlineData("H", 0, 0.5, true, "Hh::")]
        [InlineData("H", 1, 0.5, true, "Hh:1:0")]
        [InlineData("H", 2, 0.5, true, "Hh:2:0")]
        //  Orange
        [InlineData("O", 0, 0, false, "M:0,6 euro is missing")]
        [InlineData("O", 0, 0.6, false, "O::")]
        [InlineData("O", 0, 1, false, "O::")]
        public void Make_Beverage_When_The_Minimum_Amount_Is_Set(string orderType, int sugarCount, decimal amount, bool extraHot, string expected)
        {
            // Arrange
            Command order;

            if (orderType == "T")
                order = new TeaCommand(sugarCount, sugarCount > 0, extraHot);
            else if (orderType == "C")
                order = new CoffeeCommand(sugarCount, sugarCount > 0, extraHot);
            else if (orderType == "H")
                order = new ChocolateCommand(sugarCount, sugarCount > 0, extraHot);
            else if (orderType == "H")
                order = new ChocolateCommand(sugarCount, sugarCount > 0, extraHot);
            else //if (orderType == "O")
                order = new OrangeCommand();

            // Act
            string actual = _coffeeMachine.Instruct(order, amount);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_Command_To_Report()
        {
            // Arrange
            List<BeverageCommand> commands = new List<BeverageCommand>();
            commands.Add(new TeaCommand(0, false, false));
            commands.Add(new TeaCommand(1, false, false));
            commands.Add(new TeaCommand(2, true, false));
            commands.Add(new TeaCommand(2, true, false));
            commands.Add(new TeaCommand(0, true, true));
            commands.Add(new TeaCommand(1, true, true));
            commands.Add(new TeaCommand(2, true, true));
            commands.Add(new OrangeCommand());

            // Act
            foreach (BeverageCommand command in commands)
                _coffeeMachine.Instruct(command, command.Price);

            List<ReportCommand> actualReport = _repository.GetReport();

            // Assert
            Assert.Equal(commands.Count, actualReport.Count);
        }


        [Fact]
        public void Notify_User_About_A_Chocolate_Shortage_And_Send_An_Email()
        {
            // Arrange
            _mockedBeverageQuantityChecker
                .Setup(x => x.IsEmpty(It.IsAny<string>()))
                .Returns(true);

            string expectedMessage = "M:There's a shortage with 'Chocolate'. A notification was sent to our customer service.";

            // Act
            BeverageCommand command = new ChocolateCommand(0, false, false);
            string instruction = _coffeeMachine.Instruct(command, command.Price);

            // Assert
            Assert.Equal(instruction, expectedMessage);
            _mockedEmailNotifier.Verify(x => x.NotifyMissingDrink("H"), Times.Once);

        }

        [Fact]
        public void Notify_User_About_A_Coffee_Shortage_And_Send_An_Email()
        {
            // Arrange
            _mockedBeverageQuantityChecker
                .Setup(x => x.IsEmpty(It.IsAny<string>()))
                .Returns(true);

            string expectedMessage = "M:There's a shortage with 'Coffee'. A notification was sent to our customer service.";

            // Act
            BeverageCommand command = new CoffeeCommand(0, false, false);
            string instruction = _coffeeMachine.Instruct(command, command.Price);

            // Assert
            Assert.Equal(instruction, expectedMessage);
            _mockedEmailNotifier.Verify(x => x.NotifyMissingDrink("C"), Times.Once);

        }

        [Fact]
        public void Notify_User_About_A_Tea_Shortage_And_Send_An_Email()
        {
            // Arrange
            _mockedBeverageQuantityChecker
                .Setup(x => x.IsEmpty(It.IsAny<string>()))
                .Returns(true);

            string expectedMessage = "M:There's a shortage with 'Tea'. A notification was sent to our customer service.";

            // Act
            BeverageCommand command = new TeaCommand(0, false, false);
            string instruction = _coffeeMachine.Instruct(command, command.Price);

            // Assert
            Assert.Equal(instruction, expectedMessage);
            _mockedEmailNotifier.Verify(x => x.NotifyMissingDrink("T"), Times.Once);

        }

        [Fact]
        public void Notify_User_About_A_Orange_Shortage_And_Send_An_Email()
        {
            // Arrange
            _mockedBeverageQuantityChecker
                .Setup(x => x.IsEmpty(It.IsAny<string>()))
                .Returns(true);

            string expectedMessage = "M:There's a shortage with 'Orange'. A notification was sent to our customer service.";

            // Act
            BeverageCommand command = new OrangeCommand();
            string instruction = _coffeeMachine.Instruct(command, command.Price);

            // Assert
            Assert.Equal(instruction, expectedMessage);
            _mockedEmailNotifier.Verify(x => x.NotifyMissingDrink("O"), Times.Once);

        }
    }
}
