using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calculation.Controllers;

[ApiController]
[Route("calc/[controller]")]
public class CalculatorSquareController : ControllerBase
{
    private readonly ICalculationService _calculationService;
    
    public CalculatorSquareController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    private CalculatorSquareEntity MapCustomerObject(CalculatorSquareDto calculatorSquareDto)
    {
        
        var customer = new CalculatorSquareEntity();
        customer.Operand = calculatorSquareDto.Operand;
        customer.Degree = calculatorSquareDto.Degree;
        return customer;
    }

    [HttpPost("square")]
    public async Task<ActionResult<double>> Square(CalculatorSquareDto calculatorSquareDto)
    {
        var calculation = MapCustomerObject(calculatorSquareDto);
        var result = await _calculationService.Square(calculation);

        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Result);
    }
}