using System;
using System.Threading.Tasks;
using CurrencyRateGateway.Application.Queries;
using CurrencyRateGateway.Application.Queries.RateQuery;
using CurrencyRateGateway.Web.Extensions;
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
        public async Task<IActionResult> GetCurrencies([FromQuery] string currencyCode, [FromQuery] DateTime? date)
        {
            var query = new GetRatesQuery
            {
                Code = currencyCode,
                Date = date
            };

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
    }
}