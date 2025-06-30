using System;
using System.Globalization;
using System.Text;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;

namespace CurrencyRateGateway.Application.Common.UrlBuilder
{
    public class UrlBuilder
    {
        private readonly StringBuilder _stringBuilder;

        public static Result<UrlBuilder, CurrencyRateError> TryCreate(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Result.Failure<UrlBuilder, CurrencyRateError>(ErrorCodes.EmptyServiceUrl.ToDomainError());
            
            return Result.Success<UrlBuilder, CurrencyRateError>(new UrlBuilder(url));
        }

        private UrlBuilder(string url)
        {
            _stringBuilder = new StringBuilder();
            _stringBuilder.Append(url);
        }

        public UrlBuilder AddDate(string key, DateTime? date)
        {
            var formattedDate = (date ?? DateTime.Now).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _stringBuilder.Append($"?{key}={formattedDate}");
            return this;
        }
        
        public string Build() => _stringBuilder.ToString();
    }
}