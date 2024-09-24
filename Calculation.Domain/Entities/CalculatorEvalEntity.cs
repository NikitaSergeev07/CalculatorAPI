namespace Calculation.Domain.Entities;

public class CalculatorEvalEntity
{
    public string Expression { get; set; } = String.Empty;
    public double Result { get; set; }
    public Error.Error? Error { get; set; } 
}