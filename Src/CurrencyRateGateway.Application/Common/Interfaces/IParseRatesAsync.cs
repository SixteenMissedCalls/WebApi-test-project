using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;
using CurrencyRateGateway.Entities.Models;
namespace CurrencyRateGateway.Application.Common.Interfaces
{
    public interface IParseRatesAsync
    {
        public Result<List<CurrencyRate>, CurrencyRateError> Parse(Stream data, string code);
    }
}