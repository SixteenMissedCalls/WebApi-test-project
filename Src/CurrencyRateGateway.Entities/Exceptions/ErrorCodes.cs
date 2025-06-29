#nullable enable
using System;
using FluentResults;
using Sstv.DomainExceptions;

namespace CurrencyRateGateway.Entities.Exceptions
{
    [ExceptionConfig(ClassName = "CurrencyRateException")]
    [ErrorDescription(Prefix = "CR", Level = Level.Medium)]
    public enum ErrorCodes
    {
        [ErrorDescription(Description = "Unknown error", Level = Level.Critical)]
        Default = 0,

        [ErrorDescription(Description = "Currency rate not found", Level = Level.NotError)]
        CurrencyRateNotFound = 1,

        [ErrorDescription(Description = "Invalid currency code", Level = Level.Medium)]
        InvalidCurrencyCode = 2,

        [ErrorDescription(Description = "Invalid date", Level = Level.Medium)]
        InvalidDate = 3,

        [ErrorDescription(Description = "Bank of Russia service unavailable", Level = Level.Critical)]
        BankServiceUnavailable = 4,

        [ErrorDescription(Description = "Invalid data format from bank", Level = Level.Critical)]
        InvalidBankDataFormat = 5
    }
    
    public sealed class CurrencyRateError : Error
    {
        public ErrorCodes ErrorCode { get; }
        public Guid ErrorId { get; }

        public CurrencyRateError(ErrorCodes errorCode, Exception? innerException = null)
        {
            ErrorId = Guid.NewGuid();
            ErrorCode = errorCode;
        
            var description = errorCode.GetDescription();
            Message = $"{description.ErrorCode}: {description.Description}";
        
            Metadata.Add("Code", description.ErrorCode);
            Metadata.Add("Level", description.Level.ToString());
            Metadata.Add("ErrorId", ErrorId);

            if (innerException != null)
            {
                CausedBy(innerException);
            }
        }
    }
    
    public static class ErrorCodeExtensions
    {
        public static CurrencyRateError ToDomainError(this ErrorCodes errorCodes, Exception? innerException = null)
        {
            return new CurrencyRateError(errorCodes, innerException);
        }
    }
}