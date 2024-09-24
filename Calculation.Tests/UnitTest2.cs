using Calculation.Controllers;
using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Calculation.Domain.Error;

namespace Calculation.Tests
{
    [TestFixture]
    public class CalculatorPowControllerTests
    {
        private Mock<ICalculationService> _calculationServiceMock;
        private CalculatorPowController _controller;

        [SetUp]
        public void Setup()
        {
            _calculationServiceMock = new Mock<ICalculationService>();
            _controller = new CalculatorPowController(_calculationServiceMock.Object);
        }

        [Test]
        public async Task Pow_ReturnsOkResult_WhenCalculationIsSuccessful()
        {
            // Arrange
            var calculatorPowDto = new CalculatorPowDto
            {
                Operand = 2,
                Degree = 3
            };

            var calculationEntity = new CalculatorPowEntity
            {
                Operand = 2,
                Degree = 3,
                Result = 8.0,
                Error = null 
            };

            _calculationServiceMock
                .Setup(service => service.Pow(It.IsAny<CalculatorPowEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Pow(calculatorPowDto);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(8.0, okResult.Value);
        }

        [Test]
        public async Task Pow_ReturnsBadRequest_WhenCalculationFails()
        {
            // Arrange
            var calculatorPowDto = new CalculatorPowDto
            {
                Operand = 0,
                Degree = -1
            };

            var calculationEntity = new CalculatorPowEntity
            {
                Operand = 0,
                Degree = -1,
                Error = new Error("Возведение 0 в отрицательную степень невозможно", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Pow(It.IsAny<CalculatorPowEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Pow(calculatorPowDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Возведение 0 в отрицательную степень невозможно", returnedError.Message);
        }

        [Test]
        public async Task Pow_ReturnsBadRequest_WhenNegativeOperandFractionalDegree()
        {
            // Arrange
            var calculatorPowDto = new CalculatorPowDto
            {
                Operand = -4,
                Degree = 0.5 
            };

            var calculationEntity = new CalculatorPowEntity
            {
                Operand = -4,
                Degree = 0.5,
                Error = new Error("Возведение отрицательного числа в дробную степень невозможно", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Pow(It.IsAny<CalculatorPowEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Pow(calculatorPowDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Возведение отрицательного числа в дробную степень невозможно", returnedError.Message);
        }
    }
}
