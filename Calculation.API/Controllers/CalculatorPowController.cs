using Calculation.Domain.Entities;
using Calculation.Dtos;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calculation.Controllers;

[ApiController]
[Route("calc/[controller]")]
public class CalculatorPowController : ControllerBase
{
    private readonly ICalculationService _calculationService;
    
    public CalculatorPowController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    private CalculatorPowEntity MapCustomerObject(CalculatorPowDto calculatorPowDto)
    {
        
        var customer = new CalculatorPowEntity();
        customer.Operand = calculatorPowDto.Operand;
        customer.Degree = calculatorPowDto.Degree;
        return customer;
    }

    [HttpPost("pow")]
    public async Task<ActionResult<double>> Pow(CalculatorPowDto calculatorPowDto)
    {
        var calculation = MapCustomerObject(calculatorPowDto);
        var result = await _calculationService.Pow(calculation);

        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Result);
    }
}