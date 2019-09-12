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
        private RestServiceClient restClient;
        private readonly ILogger<MRZReaderHandler> _logger;
        private CloudOcrSettings _cloudOcrSettings;
        private readonly IDocumentRepository _documentRepository;
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
            _cloudOcrSettings = settings.Value;
            restClient.ApplicationId = _cloudOcrSettings.ApplicationId;
            restClient.Password = _cloudOcrSettings.Password;
        }
        protected override void Handle(MrzDocumentRequest request)
        {
            PopulatDocumentName(request);
            PopulateFileUniqueName(request);
            UploadFile(request);
            PopulateSourceFilePath(request);
            PopulateOutputFilePath(request);
            PersistSourceFileInfoInDatabase(request);
            ProcessMrz(request);
        }
        internal MrzDocumentRequest PopulatDocumentName(MrzDocumentRequest request)
        {
            if (request.ShouldContinue)
                request.DocumentName = request.Document.FileName;

            return request;
        }
        internal MrzDocumentRequest PopulateFileUniqueName(MrzDocumentRequest request)
        {
            //// Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
            if (request.ShouldContinue)
                request.FileUniqueName = Guid.NewGuid().ToString() + "_" + request.DocumentName;

            return request;
        }
        internal MrzDocumentRequest UploadFile(MrzDocumentRequest request)
        {
            if (request.ShouldContinue)
            {
                //Create the document.
                string sourceFile = Path.Combine(request.SourceFolder, request.FileUniqueName);
                using (FileStream fileStream = System.IO.File.Create(sourceFile))
                {
                    request.Document.CopyTo(fileStream);
                }
            }
            return request;
        }
        internal MrzDocumentRequest PopulateSourceFilePath(MrzDocumentRequest request)
        {
            if (request.ShouldContinue)
                request.SourceFilePath = $"{request.SourceFolder}\\{request.FileUniqueName}";

            return request;
        }
        internal MrzDocumentRequest PopulateOutputFilePath(MrzDocumentRequest request)
        {
            if (request.ShouldContinue)
            {
                var fileWithoutExtension = Path.GetFileNameWithoutExtension($"{request.DestinationFolder}\\{request.FileUniqueName}");
                var _outputFilePath = $"{request.DestinationFolder}\\{fileWithoutExtension}.xml";
                request.OutputFilePath = _outputFilePath;
            }
            return request;
        }
        internal MrzDocumentRequest PersistSourceFileInfoInDatabase(MrzDocumentRequest request)
        {
            _documentRepository.Add(new Document()
            {
                DocumentExtension = "xml",
                DocumentLocation = request.OutputFilePath
            });
            return request;
        }
        internal void ProcessMrz(MrzDocumentRequest request)
        {
            try
            {
                OcrSdkTask task = restClient.ProcessMrz(request.SourceFilePath);
                WaitAndDownload(task, request);
            }
            catch (Exception e)
            {
                _logger.LogTrace($"{e.Message}");
                throw;
            }
        }
        internal void WaitAndDownload(OcrSdkTask task, MrzDocumentRequest request)
        {
            task = WaitForTask(task);

            if (task.Status == Abbyy.CloudOcrSdk.TaskStatus.Completed)
            {
                _logger.LogTrace("Processing completed.");
                restClient.DownloadResult(task, request.OutputFilePath);
                _logger.LogTrace("Download completed.");
            }
            else
            {
                _logger.LogTrace("Error while processing the task");
            }
        }
        internal OcrSdkTask WaitForTask(OcrSdkTask task)
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