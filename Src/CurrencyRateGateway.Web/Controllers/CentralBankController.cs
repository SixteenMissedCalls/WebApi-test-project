using System;
using System.Threading.Tasks;
using CurrencyRateGateway.Application.Common.Dto;
using CurrencyRateGateway.Application.Queries.RateQuery;
using CurrencyRateGateway.Entities.Models;
using CurrencyRateGateway.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Получение курсов валют",
            Description = "Возвращает курс указанной валюты или список всех валют на указанную дату"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешный запрос", typeof(CurrencyRate))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Курс валюты не найден")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неверные параметры запроса", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Ошибка на стороне сервера")]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Сервис Банка России недоступен")]
        public async Task<IActionResult> GetCurrencies(
            [FromQuery, SwaggerParameter("Код валюты (например USD, EUR)", Required = false)]
            string currencyCode,
            [FromQuery, SwaggerParameter("Дата курса в формате YYYY-MM-DD", Required = false)] DateTime? date)
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