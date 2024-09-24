namespace Calculation.Domain.Entities;

public class CalculatorSquareEntity
{
    public double Operand { get; set; }  
    public double Degree { get; set; } 
    public double Result { get; set; } 
    public Error.Error? Error { get; set; } 
}