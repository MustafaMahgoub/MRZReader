using Abbyy.CloudOcrSdk;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MRZReader.Core.ReaderHandlers
{
    public class MRZReaderHandler : RequestHandler<MrzDocumentRequest>
    {
        private RestServiceClient restClient;
        private readonly ILogger<MRZReaderHandler> _logger;
        private CloudOcrSettings _cloudOcrSettings;
        private string _outputFileName;
        private string _outputFilePath;

        public MRZReaderHandler(ILoggerFactory loggerFactory, IOptions<CloudOcrSettings> settings)
        {
            _logger = loggerFactory.CreateLogger<MRZReaderHandler>();
            PopulateCloudOcrSettingsSettings(settings);
        }
        public void PopulateCloudOcrSettingsSettings(IOptions<CloudOcrSettings> settings)
        {
            restClient = new RestServiceClient();
            restClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
            _cloudOcrSettings =settings.Value;
            restClient.ApplicationId = _cloudOcrSettings.ApplicationId;
            restClient.Password = _cloudOcrSettings.Password; 
        }
        protected override void Handle(MrzDocumentRequest request)
        {
            PopulateOutputFileName(request.SourceFilePath);
            PopulateOutputFilePath(request.SourceFilePath);
            ProcessMrz(request.SourceFilePath);
        }
        private void PopulateOutputFileName(string sourceFilePath)
        {
           _outputFileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        }
        private void PopulateOutputFilePath(string filePath)
        {
            _outputFilePath = Path.Combine(_cloudOcrSettings.OutputPath, _outputFileName + ".xml");
        }
        public void ProcessMrz(string sourceFilePath)
        {
            OcrSdkTask task = restClient.ProcessMrz(sourceFilePath);
            // Log
            WaitAndDownload(task);
        }
        public void WaitAndDownload(OcrSdkTask task)
        {
            if (task.Status == Abbyy.CloudOcrSdk.TaskStatus.Completed)
            {
                // Log
                restClient.DownloadResult(task, _outputFilePath);
                // Log
            }
            else
            {
                // Log
            }
        }
    }
}
