using Abbyy.CloudOcrSdk;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;

namespace MRZReader.Core.ReaderHandlers
{
    public class MRZReaderHandler
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
        public void Handle(string sourceFilePath)
        {
            PopulateOutputFileName(sourceFilePath);
            PopulateOutputFilePath(sourceFilePath);
            ProcessMrz(sourceFilePath);
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
            if (task.Status == TaskStatus.Completed)
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
