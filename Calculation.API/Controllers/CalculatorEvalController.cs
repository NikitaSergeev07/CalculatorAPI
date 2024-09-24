using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calculation.Controllers;

[ApiController]
[Route("calc/[controller]")]
public class CalculatorEvalController : ControllerBase
{
    private readonly ICalculationService _calculationService;
    
    public CalculatorEvalController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    private CalculatorEvalEntity MapCustomerObject(CalculatorEvalDto calculatorEvalDto)
    {
        
        var customer = new CalculatorEvalEntity();
        customer.Expression = calculatorEvalDto.Expression;
        return customer;
    }

    [HttpPost("eval")]
    public async Task<ActionResult<double>> Eval(CalculatorEvalDto calculatorEvalDto)
    {
        var calculation = MapCustomerObject(calculatorEvalDto);
        var result = await _calculationService.Eval(calculation);

        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Result);
    }
}