namespace Calculation.Domain.Error;

public class Error
{
    public string Message { get; set; } = string.Empty; 
    public int Code { get; set; } 

    public Error()
    {
    }

    public Error(string message, int code)
    {
        Message = message;
        Code = code;
    }
}