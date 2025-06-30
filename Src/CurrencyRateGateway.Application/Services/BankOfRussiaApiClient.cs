using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CurrencyRateGateway.Application.Common.Interfaces;
using CurrencyRateGateway.Entities.Exceptions;

namespace CurrencyRateGateway.Application.Services
{
    public class BankOfRussiaApiClient : IBankOfRussiaApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BankOfRussiaApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<Stream, CurrencyRateError>> GetRatesAsync(string url, CancellationToken token = default)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);
            
            if(!response.IsSuccessStatusCode)
                return Result.Failure<Stream, CurrencyRateError>(ErrorCodes.BankServiceUnavailable.ToDomainError());
            
            return await response.Content.ReadAsStreamAsync();
        }
    }
}