using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Application.Queries.RateQuery;
using CurrencyRateGateway.Entities.Exceptions;
using CurrencyRateGateway.Entities.Models;
using MediatR;
using Serilog;

namespace CurrencyRateGateway.Application.Pipelines
{
    public class ExceptionHandlingForGetRatesQueryBehavior<TRequest, T> 
        : IPipelineBehavior<TRequest, Result<T, CurrencyRateError>>
    {
        private readonly ILogger _logger;

        public ExceptionHandlingForGetRatesQueryBehavior(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task<Result<T, CurrencyRateError>> Handle(TRequest request, RequestHandlerDelegate<Result<T, CurrencyRateError>> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling {RequestType}", nameof(GetRatesQuery));
                return Result.Failure<T, CurrencyRateError>(ErrorCodes.Default.ToDomainError());
            }
        }
    }
}

