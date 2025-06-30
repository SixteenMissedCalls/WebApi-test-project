using System;
using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateGateway.Web.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T, CurrencyRateError> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return ErrorToActionResult(result.Error);
        }

        private static IActionResult ErrorToActionResult(CurrencyRateError error)
        {
            var errorResponse = new
            {
                error.Code,
                error.Message
            };

            switch (error.Code)
            {
                case ErrorCodes.CurrencyRateNotFound:
                    return new NoContentResult();

                case ErrorCodes.InvalidCurrencyCode:
                case ErrorCodes.InvalidDate:
                    return new BadRequestObjectResult(errorResponse);

                case ErrorCodes.BankServiceUnavailable:
                case ErrorCodes.EmptyServiceUrl:
                    return new ObjectResult(errorResponse)
                    {
                        StatusCode = (int)HttpStatusCode.ServiceUnavailable
                    };

                default:
                    return new ObjectResult(errorResponse)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
            }
        }
    }
}