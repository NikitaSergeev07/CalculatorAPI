using Calculation.Domain.Entities;
using Calculation.Domain.Error;
using Calculation.Services.Interfaces;
using Calculation.Services.PolishNotation;

namespace Calculation.Services.Implementations;

public class CalculationService : ICalculationService
{
    public async Task<CalculatorEntity> Calculate(CalculatorEntity calculator)
    {
        double result;
        
        try
        {
            switch (calculator.Operator)
            {
                case "+":
                    result = calculator.Operand1 + calculator.Operand2;
                    break;
                case "-":
                    result = calculator.Operand1 - calculator.Operand2;
                    break;
                case "*":
                    result = calculator.Operand1 * calculator.Operand2;
                    break;
                case "/":
                    if (calculator.Operand2 == 0)
                    {
                        calculator.Error = new Error("Деление на ноль невозможно", 400);
                        return calculator;
                    }
                    result = calculator.Operand1 / calculator.Operand2;
                    break;
                default:
                    calculator.Error = new Error("Некорректная операция", 400);
                    return calculator;
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                calculator.Error = new Error("Результат слишком большой или некорректный", 400);
                return calculator;
            }

            calculator.Result = result;
        }
        catch (Exception ex)
        {
            calculator.Error = new Error($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }

    public async Task<CalculatorPowEntity> Pow(CalculatorPowEntity calculator)
    {
        if (calculator.Operand == 0 && calculator.Degree < 0)
        {
            calculator.Error = new Error("Возведение 0 в отрицательную степень невозможно", 400);
            return calculator;
        }
        if (calculator.Operand < 0 && calculator.Degree % 1 != 0)
        {
            calculator.Error = new Error("Возведение отрицательного числа в дробную степень невозможно", 400);
            return calculator;
        }
        try
        {
            double result = Math.Pow(calculator.Operand, calculator.Degree);

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                calculator.Error = new Error("Результат слишком большой или некорректный", 400);
                return calculator;
            }

            calculator.Result = result;
        }
        catch (Exception ex)
        {
            calculator.Error = new Error($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }

    public async Task<CalculatorSquareEntity> Square(CalculatorSquareEntity calculator)
    {
        if (calculator.Degree == 0)
        {
            calculator.Error = new Error("Степень не может быть равна нулю", 400);
            return calculator;
        }

        if (calculator.Operand < 0 && calculator.Degree % 2 == 0)
        {
            calculator.Error = new Error("Извлечение корня из отрицательного числа невозможно", 400);
            return calculator;
        }

        try
        {
            double result = Math.Pow(calculator.Operand, 1.0 / calculator.Degree);

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                calculator.Error = new Error("Результат слишком большой или некорректный", 400);
                return calculator;
            }

            calculator.Result = result;
        }
        catch (Exception ex)
        {
            calculator.Error = new Error($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }

    public async Task<CalculatorEvalEntity> Eval(CalculatorEvalEntity calculator)
    {
        try
        {
            var (result, error) = RPN.Calculate(calculator.Expression);

            if (error != null)
            {
                calculator.Error = error; 
                return calculator;
            }

            if (double.IsInfinity(result.Value) || double.IsNaN(result.Value))
            {
                calculator.Error = new Error("Результат слишком большой или некорректный", 400);
                return calculator;
            }

            calculator.Result = result.Value;
        }
        catch (Exception ex)
        {
            calculator.Error = new Error($"Произошла ошибка при вычислении: {ex.Message}", 500);
        }

        return await Task.FromResult(calculator);
    }

}