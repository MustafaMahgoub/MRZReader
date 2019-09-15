using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trulioo.Client.V1;

namespace MRZReader.Handlers.KYCHandlers
{
    public class TruliooHandler 
    {
        private readonly ILogger _logger;
        private TruliooSettings _truliooSettings;

        public TruliooHandler(ILogger<TruliooHandler> logger, IOptions<TruliooSettings> settings)
        {
            _logger = logger;
            _truliooSettings = settings.Value;
        }
        internal async Task<DocumentRequest> Handle(DocumentRequest request)
        {
            try
            {
                var truliooClient = new TruliooApiClient(_truliooSettings.Username, _truliooSettings.Password);
                var responseString = await truliooClient.Connection.SayHelloAsync("Joe Napoli");
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                request.IsSuccessed = false;
                throw;
            }
            return request;
        }

        private void Log(string msg, bool isEception = false)
        {
            if (isEception)
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
