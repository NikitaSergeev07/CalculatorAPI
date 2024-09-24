using Calculation.Controllers;
using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Calculation.Domain.Error;

namespace Calculation.Tests
{
    [TestFixture]
    public class CalculatorControllerTests
    {
        private Mock<ICalculationService> _calculationServiceMock;
        private CalculatorController _controller;

        [SetUp]
        public void Setup()
        {
            _calculationServiceMock = new Mock<ICalculationService>();
            _controller = new CalculatorController(_calculationServiceMock.Object);
        }

        [Test]
        public async Task Calculate_ReturnsOkResult_WhenCalculationIsSuccessful()
        {
            // Arrange
            var calculatorDto = new CalculatorDto
            {
                Operand1 = 10,
                Operand2 = 5,
                Operator = "+"
            };

            var calculationEntity = new CalculatorEntity
            {
                Operand1 = 10,
                Operand2 = 5,
                Operator = "+",
                Result = 15.0,
                Error = null 
            };

            _calculationServiceMock
                .Setup(service => service.Calculate(It.IsAny<CalculatorEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Calculate(calculatorDto);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(15.0, okResult.Value);

            Assert.IsNull(calculationEntity.Error);
        }


        [Test]
        public async Task Calculate_ReturnsBadRequest_WhenCalculationFails()
        {
            // Arrange
            var calculatorDto = new CalculatorDto
            {
                Operand1 = 10,
                Operand2 = 0,
                Operator = "/"
            };

            var calculationEntity = new CalculatorEntity
            {
                Operand1 = 10,
                Operand2 = 0,
                Operator = "/",
                Error = new Error("Деление на ноль невозможно", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Calculate(It.IsAny<CalculatorEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Calculate(calculatorDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
    
            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Деление на ноль невозможно", returnedError.Message);
        }


    }
}