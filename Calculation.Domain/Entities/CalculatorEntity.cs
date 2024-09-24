

namespace Calculation.Domain.Entities;

public class CalculatorEntity
{
    public double Operand1 { get; set; }  
    public double Operand2 { get; set; } 
    public string Operator { get; set; }  
    public double Result { get; set; } 
    public Error.Error? Error { get; set; }  
}
