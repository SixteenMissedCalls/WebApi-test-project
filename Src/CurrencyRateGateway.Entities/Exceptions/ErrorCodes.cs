#nullable enable
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Sstv.DomainExceptions;

namespace CurrencyRateGateway.Entities.Exceptions
{
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
        
        [ErrorDescription(Description = "Invalid url format for bank api", Level = Level.Critical)]
        EmptyServiceUrl = 6
    }
    
    public sealed class CurrencyRateError 
    {
        public ErrorCodes Code { get; }
        
        public string Message { get; }
        
        public LogLevel LogLevel { get; }

        public CurrencyRateError(ErrorCodes errorCode)
        {
            Code = errorCode;
            Message = $"{errorCode}: {GetDefaultMessage(errorCode)}";
            LogLevel = GetLogLevel(errorCode);
        }
        
        private static string GetDefaultMessage(ErrorCodes code)
        {
            var memberInfo = typeof(ErrorCodes).GetMember(code.ToString()).FirstOrDefault();
            var attribute = memberInfo?.GetCustomAttribute<ErrorDescriptionAttribute>();
            return attribute?.Description ?? "Unknown error";
        }
        
        private static LogLevel GetLogLevel(ErrorCodes code)
        {
            var memberInfo = typeof(ErrorCodes).GetMember(code.ToString()).FirstOrDefault();
            var attribute = memberInfo?.GetCustomAttribute<ErrorDescriptionAttribute>();
            var level = attribute?.Level ?? Level.Medium;

            return ConvertToLogLevel(level);
        }
        
        private static LogLevel ConvertToLogLevel(Level level) => level switch
        {
            Level.NotError => LogLevel.Information,
            Level.Low => LogLevel.Warning,
            Level.Medium => LogLevel.Warning,
            Level.Fatal => LogLevel.Critical,
            _ => LogLevel.Error
        };
    }
    
    public static class ErrorCodeExtensions
    {
        public static CurrencyRateError ToDomainError(this ErrorCodes errorCodes)
        {
            return new CurrencyRateError(errorCodes);
        }
    }
}