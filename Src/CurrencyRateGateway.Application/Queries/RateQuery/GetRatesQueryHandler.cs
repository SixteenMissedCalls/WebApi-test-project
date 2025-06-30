using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Application.Common.Interfaces;
using CurrencyRateGateway.Application.Common.UrlBuilder;
using CurrencyRateGateway.Application.Validators;
using CurrencyRateGateway.Entities.Exceptions;
using CurrencyRateGateway.Entities.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CurrencyRateGateway.Application.Queries.RateQuery
{
    public class GetRatesQueryHandler : IRequestHandler<GetRatesQuery, Result<List<CurrencyRate>, CurrencyRateError>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBankOfRussiaApiClient _bankOfRussiaApiClient;
        private readonly IParseRatesAsync _parseRatesAsync;
        private readonly ILogger<GetRatesQueryHandler> _logger;

        public GetRatesQueryHandler(IConfiguration configuration,
            IBankOfRussiaApiClient bankOfRussiaApiClient,
            IParseRatesAsync parseRatesAsync, ILogger<GetRatesQueryHandler> logger)
        {
            _configuration = configuration;
            _bankOfRussiaApiClient = bankOfRussiaApiClient;
            _parseRatesAsync = parseRatesAsync;
            _logger = logger;
        }

        public async Task<Result<List<CurrencyRate>, CurrencyRateError>> Handle(GetRatesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = CurrencyValidator.ValidateCurrencyCode(request.Code)
                    .Bind(() => CurrencyValidator.ValidateDate(request.Date));

                if (validation.IsFailure)
                    return Result.Failure<List<CurrencyRate>, CurrencyRateError>(validation.Error);

                var urlBuilder = UrlBuilder.TryCreate(_configuration["CentralBankHost"]);

                if (urlBuilder.IsFailure)
                    return Result.Failure<List<CurrencyRate>, CurrencyRateError>(urlBuilder.Error);

                var url = urlBuilder.Value
                    .AddDate("date_req", request.Date)
                    .Build();

                return await _bankOfRussiaApiClient.GetRatesAsync(url, cancellationToken)
                    .Bind(r => _parseRatesAsync.Parse(r, request.Code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {name}", nameof(GetRatesQueryHandler));
                return Result.Failure<List<CurrencyRate>, CurrencyRateError>(ErrorCodes.Default.ToDomainError());
            }
        }
    }
}