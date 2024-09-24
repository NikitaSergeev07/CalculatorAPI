using Calculation.Domain.Entities;
using Calculation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Calculation.Dtos;

namespace Calculation.Controllers;

[ApiController]
[Route("calc/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ICalculationService _calculationService;
    
    public CalculatorController(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    private CalculatorEntity MapCustomerObject(CalculatorDto calculatorDto)
    {
        
        var customer = new CalculatorEntity();
        customer.Operand1 = calculatorDto.Operand1;
        customer.Operand2 = calculatorDto.Operand2;
        customer.Operator = calculatorDto.Operator;
        return customer;
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<double>> Calculate(CalculatorDto calculatorDto)
    {
        var calculation = MapCustomerObject(calculatorDto);
        var result = await _calculationService.Calculate(calculation);

        if (result.Error != null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Result);
    }
}