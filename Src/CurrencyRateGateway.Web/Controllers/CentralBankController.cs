using System;
using System.Threading.Tasks;
using CurrencyRateGateway.Application.Queries;
using FluentResults.Extensions.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateGateway.Web.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrencies([FromQuery] string currencyCode, [FromQuery] DateTime? date)
        {
            var query = new GetRatesQuery
            {
                Code = currencyCode,
                Date = date
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result);
            
            return 

        }
    }
}