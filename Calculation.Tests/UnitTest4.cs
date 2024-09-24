using Calculation.Controllers;
using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Calculation.Domain.Error;

namespace Calculation.Tests
{
    [TestFixture]
    public class CalculatorEvalControllerTests
    {
        private Mock<ICalculationService> _calculationServiceMock;
        private CalculatorEvalController _controller;

        [SetUp]
        public void Setup()
        {
            _calculationServiceMock = new Mock<ICalculationService>();
            _controller = new CalculatorEvalController(_calculationServiceMock.Object);
        }

        [Test]
        public async Task Eval_ReturnsOkResult_WhenEvaluationIsSuccessful()
        {
            // Arrange
            var calculatorEvalDto = new CalculatorEvalDto
            {
                Expression = "5 - 6*4^(4/2)"
            };

            var calculationEntity = new CalculatorEvalEntity
            {
                Expression = "5 - 6*4^(4/2)",
                Result = -91,
                Error = null
            };

            _calculationServiceMock
                .Setup(service => service.Eval(It.IsAny<CalculatorEvalEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Eval(calculatorEvalDto);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(-91, okResult.Value);
        }

        [Test]
        public async Task Eval_ReturnsBadRequest_WhenEvaluationFails_WithSyntaxError()
        {
            // Arrange
            var calculatorEvalDto = new CalculatorEvalDto
            {
                Expression = "3++5"
            };

            var calculationEntity = new CalculatorEvalEntity
            {
                Expression = "3++5",
                Error = new Error("Синтаксическая ошибка в выражении", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Eval(It.IsAny<CalculatorEvalEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Eval(calculatorEvalDto);

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
    
            var returnedError = badRequestResult.Value as Error;
            Assert.IsNotNull(returnedError);
            Assert.AreEqual("Синтаксическая ошибка в выражении", returnedError.Message);
        }

        [Test]
        public async Task Eval_ReturnsBadRequest_WhenEvaluationFails_WithDivisionByZero()
        {
            // Arrange
            var calculatorEvalDto = new CalculatorEvalDto
            {
                Expression = "10/0"
            };

            var calculationEntity = new CalculatorEvalEntity
            {
                Expression = "10/0",
                Error = new Error("Деление на ноль невозможно", 400)
            };

            _calculationServiceMock
                .Setup(service => service.Eval(It.IsAny<CalculatorEvalEntity>()))
                .ReturnsAsync(calculationEntity);

            // Act
            var result = await _controller.Eval(calculatorEvalDto);

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
