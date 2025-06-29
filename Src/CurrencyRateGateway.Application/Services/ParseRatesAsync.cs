using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Application.Common.Dto;
using CurrencyRateGateway.Application.Common.Interfaces;
using CurrencyRateGateway.Entities.Exceptions;
using CurrencyRateGateway.Entities.Models;

namespace CurrencyRateGateway.Application.Services
{
    public class ParseRatesAsync : IParseRatesAsync
    {
        public Result<List<CurrencyRate>, CurrencyRateError> Parse(Stream stream, string code)
        {
            var deserializer = new XmlSerializer(typeof(ValCursXmlDto));
            var dto = (ValCursXmlDto)deserializer.Deserialize(stream);

            var entities = dto.Valutes.Select(v => v.ToDomain(dto.Date)).ToList();
            var result = string.IsNullOrEmpty(code)
                ? entities
                : entities.Where(x => string.Equals(x.CharCode, code, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (!result.Any())
                return Result.Failure<List<CurrencyRate>, CurrencyRateError>(
                    ErrorCodes.CurrencyRateNotFound.ToDomainError());

            return Result.Success<List<CurrencyRate>, CurrencyRateError>(result);
        }
    }
}