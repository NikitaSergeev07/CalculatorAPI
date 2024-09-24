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
    public class CalculatorSquareControllerTests
    {
        private Mock<ICalculationService> _calculationServiceMock;
        private CalculatorSquareController _controller;

        [SetUp]
        public void Setup()
        {
            _calculationServiceMock = new Mock<ICalculationService>();
            _controller = new CalculatorSquareController(_calculationServiceMock.Object);
        }

        [Test]
        public async Task Square_ReturnsOkResult_WhenCalculationIsSuccessful()
        {
            // Arrange
            var calculatorSquareDto = new CalculatorSquareDto
            {
                Operand = 16,
                Degree = 2
            };

            var calculationEntity = new CalculatorSquareEntity
            {
                Operand = 16,
                Degree = 2,
                Result = 4.0,
                Error = null 
            };

            _calculationServiceMock
                .Setup(service => service.Square(It.IsAny<CalculatorSquareEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Square(calculatorSquareDto);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(4.0, okResult.Value);
        }

        [Test]
        public async Task Square_ReturnsBadRequest_WhenDegreeIsZero()
        {
            // Arrange
            var calculatorSquareDto = new CalculatorSquareDto
            {
                Operand = 9,
                Degree = 0
            };

            var calculationEntity = new CalculatorSquareEntity
            {
                Operand = 9,
                Degree = 0,
                Error = new Error("Степень не может быть равна нулю", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Square(It.IsAny<CalculatorSquareEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Square(calculatorSquareDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Степень не может быть равна нулю", returnedError.Message);
        }

        [Test]
        public async Task Square_ReturnsBadRequest_WhenNegativeOperandEvenDegree()
        {
            // Arrange
            var calculatorSquareDto = new CalculatorSquareDto
            {
                Operand = -16,
                Degree = 2 
            };

            var calculationEntity = new CalculatorSquareEntity
            {
                Operand = -16,
                Degree = 2,
                Error = new Error("Извлечение корня из отрицательного числа невозможно", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Square(It.IsAny<CalculatorSquareEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Square(calculatorSquareDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Извлечение корня из отрицательного числа невозможно", returnedError.Message);
        }
    }
}
