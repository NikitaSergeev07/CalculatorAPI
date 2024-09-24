using Calculation.Domain.Entities;

namespace Calculation.Services.Interfaces;

public interface ICalculationService
{
    Task<CalculatorEntity> Calculate(CalculatorEntity calculator);
    Task<CalculatorPowEntity> Pow(CalculatorPowEntity calculator);
    
    Task<CalculatorSquareEntity> Square(CalculatorSquareEntity calculator);
    
    Task<CalculatorEvalEntity> Eval(CalculatorEvalEntity calculator);
}