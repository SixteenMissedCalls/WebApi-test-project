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

namespace CurrencyRateGateway.Application.Queries
{
    public class GetRatesQueryHandler : IRequestHandler<GetRatesQuery, Result<List<CurrencyRate>, CurrencyRateError>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBankOfRussiaApiClient _bankOfRussiaApiClient;
        private readonly IParseRatesAsync _parseRatesAsync;

        public GetRatesQueryHandler(IConfiguration configuration,
            IBankOfRussiaApiClient bankOfRussiaApiClient,
            IParseRatesAsync parseRatesAsync)
        {
            _configuration = configuration;
            _bankOfRussiaApiClient = bankOfRussiaApiClient;
            _parseRatesAsync = parseRatesAsync;
        }

        public async Task<Result<List<CurrencyRate>, CurrencyRateError>> Handle(GetRatesQuery request, CancellationToken cancellationToken)
        {
            var validation = CurrencyValidator.ValidateCurrencyCode(request.Code)
                .Bind(() => CurrencyValidator.ValidateDate(request.Date));

            if (validation.IsFailure)
                return Result.Failure<List<CurrencyRate>, CurrencyRateError>(validation.Error);

            var urlBuilder = UrlBuilder.TryCreate(_configuration["CentralBankHost"]);
            
            if(urlBuilder.IsFailure)
                return Result.Failure<List<CurrencyRate>, CurrencyRateError>(urlBuilder.Error);
            
            var url = urlBuilder.Value
                .AddDate("date_req", request.Date)
                .Build();
            
            return await _bankOfRussiaApiClient.GetRatesAsync(url, cancellationToken)
                .Bind(r => _parseRatesAsync.Parse(r, request.Code));
        }
    }
}