using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Entities.Exceptions;

namespace CurrencyRateGateway.Application.Common.Interfaces
{
    public interface IBankOfRussiaApiClient
    {
        Task<Result<Stream, CurrencyRateError>> GetRatesAsync(string url, CancellationToken token = default);
    }
}