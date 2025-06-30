using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateGateway.Application.Pipelines
{
    public class LogFailuresBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        private readonly ILogger _logger;

        public LogFailuresBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next(cancellationToken);
            var requestName = typeof(TRequest).Name;

            if (response is IResult<object, CurrencyRateError> { IsFailure: true } result)
            {
                var error = result.Error;
                _logger.Log(error.LogLevel, "Request {RequestName} failed with {Error}",
                    requestName, error.Message);
            }
            
            return response;
        }
    }
}