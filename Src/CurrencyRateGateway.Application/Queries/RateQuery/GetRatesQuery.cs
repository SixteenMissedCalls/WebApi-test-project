#nullable enable
using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;
using CurrencyRateGateway.Entities.Models;
using MediatR;

namespace CurrencyRateGateway.Application.Queries.RateQuery
{
    public class GetRatesQuery : IRequest<Result<List<CurrencyRate>, CurrencyRateError>>
    {
        public DateTime? Date { get; set; }
        
        public string? Code { get; set; }
    }
}