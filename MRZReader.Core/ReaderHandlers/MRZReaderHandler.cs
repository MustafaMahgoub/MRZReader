using System;
using Abbyy.CloudOcrSdk;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;

namespace MRZReader.Core
{
    public class MRZReaderHandler : RequestHandler<MrzDocumentRequest>
    {
        private  RestServiceClient restClient;
        private readonly ILogger<MRZReaderHandler> _logger;
        private CloudOcrSettings _cloudOcrSettings;
        private readonly IDocumentRepository _documentRepository;

        private string _outputFileName;
        private string _outputFilePath;

        public MRZReaderHandler(ILoggerFactory loggerFactory, IDocumentRepository documentRepository, IOptions<CloudOcrSettings> settings)
        {
            _logger = loggerFactory.CreateLogger<MRZReaderHandler>();
            _documentRepository = documentRepository;
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
            PopulateOutputFileName(request);
            PopulateOutputFilePath(request);
            ProcessMrz(request);
            PresistDocument(request);
        }
        private void PresistDocument(MrzDocumentRequest request)
        {
            _documentRepository.Add(new Document()
            {
                DocumentExtension = "XML",
                DocumentLocation = _outputFilePath
            });
        }

        private void PopulateOutputFileName(MrzDocumentRequest request)
        {
           _outputFileName = Path.GetFileNameWithoutExtension(request.SourceFilePath);
        }
        private void PopulateOutputFilePath(MrzDocumentRequest request)
        {
            _outputFilePath = Path.Combine(request.OutputFilePath, _outputFileName + ".xml");
        }
        public void ProcessMrz(MrzDocumentRequest request)
        {
            try
            {
                OcrSdkTask task = restClient.ProcessMrz(request.SourceFilePath);
                WaitAndDownload(task);
            }
            catch (Exception e)
            {
                _logger.LogTrace($"{e.Message}");
               throw;
            }
        }
        public void WaitAndDownload(OcrSdkTask task)
        {
            task = WaitForTask(task);

            if (task.Status == Abbyy.CloudOcrSdk.TaskStatus.Completed)
            {
                _logger.LogTrace("Processing completed.");
                restClient.DownloadResult(task, _outputFilePath);
                _logger.LogTrace("Download completed.");
            }
            else
            {
                _logger.LogTrace("Error while processing the task");
            }
        }
        private OcrSdkTask WaitForTask(OcrSdkTask task)
        {
            _logger.LogTrace(string.Format("Task status: {0}", task.Status));
            while (task.IsTaskActive())
            {
                System.Threading.Thread.Sleep(5000);
                task = restClient.GetTaskStatus(task.Id);
                _logger.LogTrace(string.Format("Task status: {0}", task.Status));
            }
            return task;
        }
    }
}
