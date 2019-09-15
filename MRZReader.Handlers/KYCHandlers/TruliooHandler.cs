using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Trulioo.Client.V1;

namespace MRZReader.Handlers.KYCHandlers
{
    public class TruliooHandler : ITruliooHandler
    {
        private readonly ILogger _logger;
        private TruliooSettings _truliooSettings;
        private readonly IHttpClientFactory _HttpClientFactory;
        private HttpClient client;

        public TruliooHandler(ILogger<TruliooHandler> logger, IOptions<TruliooSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _truliooSettings = settings.Value;
            _HttpClientFactory = httpClientFactory;
            client = _HttpClientFactory.CreateClient("TruliooClient");
        }
        internal async Task<string> SayHelloAsync()
        {
            string responseString = String.Empty;
            try
            {
                var truliooClient = new TruliooApiClient(_truliooSettings.Username, _truliooSettings.Password);
                responseString = await truliooClient.Connection.SayHelloAsync("Joe Napoli");
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
            }
            return responseString;
        }
        internal async Task<DocumentRequest> TestAuthentication(DocumentRequest _documentRequest)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                //var truliooClient = new TruliooApiClient(_truliooSettings.Username, _truliooSettings.Password);
                //IEnumerable<string> e = await truliooClient.Configuration.GetCountryCodesAsync("Identity Verification");
                //responseString = await truliooClient.Connection.TestAuthenticationAsync();

                var request = new HttpRequestMessage(HttpMethod.Get, "connection/v1/testauthentication");
                response = await client.SendAsync(request);
                _documentRequest.IsTruliooAuthenticated = response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
            }
            return _documentRequest;
        }
        public async Task<DocumentRequest> Handle(DocumentRequest request)
        {
            try
            {
                // First step is test the Authentication
                await TestAuthentication(request);

                // [TODO] Series OF TASKS

            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                request.IsSuccessed = false;
                throw;
            }
            return request;
        }
        private void Log(string msg, bool isException = false)
        {
            if (isException)
            {
                _logger.LogError($"[MRZ_Logs] {msg}.");
            }
            else
            {
                _logger.LogTrace($"[MRZ_Logs] {msg}.");
            }
        }
    }
}
