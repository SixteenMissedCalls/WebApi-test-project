#nullable enable
using System;
using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;

namespace CurrencyRateGateway.Application.Validators
{
    public class CurrencyValidator
    {
        public static UnitResult<CurrencyRateError> ValidateCurrencyCode(string? code)
        {
            if (code != null && code.Length != 3)
                return UnitResult.Failure(ErrorCodes.InvalidCurrencyCode.ToDomainError());

            return UnitResult.Success<CurrencyRateError>();
        }

        public static UnitResult<CurrencyRateError> ValidateDate(DateTime? date)
        {
            if (date != null && date > DateTime.UtcNow.AddDays(1))
                return UnitResult.Failure(ErrorCodes.InvalidDate.ToDomainError());

            return UnitResult.Success<CurrencyRateError>();
        }
    }
}