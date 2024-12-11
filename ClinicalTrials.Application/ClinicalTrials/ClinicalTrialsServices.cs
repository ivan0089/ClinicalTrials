
using ClinicalTrials.Application.Helpers;
using ClinicalTrials.Application.Validations;
using ClinicalTrials.Domain;
using Microsoft.Extensions.Logging;

namespace ClinicalTrials.Application.ClinicalTrials
{
    internal class ClinicalTrialsServices(IClinicalTrialsRepository clinicalTrialsRepository, 
        IFileValidation fileValidation, 
        IJsonSchemValidation jsonSchemValidation, 
        IClinicalTrialsProcessingHandler processingHandler, 
        IClinicalTrialsMaterializeHandler materializeHandler,
        ILogger<ClinicalTrialsServices> logger) : IClinicalTrialsServices
    {
        private const string EmbeddedResouceMetadata = "ClinicalTrialMetadataSchema.json";

        public async Task<ClinicalTrial> GetTrialByIdAsync(string id)
        {
            try
            {
                return await clinicalTrialsRepository.RetrieveByIdAsync(id);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Something went worng during retreiving clinical trials by {nameof(id)}: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<ClinicalTrial>> GetClinicalTrialsByFilterOptionsAsync(ClinicalTrialFilter filter)
        {
            try
            {
                return await clinicalTrialsRepository.RetrieveByFilterOprionsAsync(filter);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Something went worng during filtering clinical trials");
                throw;
            }
        }

        public async Task<Result> HandleClinicalTrialAsync(string fileName, Stream fileContent)
        {
            try
            {
                var isFileSizeValid = fileValidation.IsValidFileSize(fileContent.Length);
                if (!isFileSizeValid)
                {
                    logger.LogDebug($"Invalid file size in bytes {fileContent.Length}");
                    return Result.FromError($"Invalid file size");
                }

                var isFileExtensionAllowed = fileValidation.IsAllowedExtension(fileName);
                if (!isFileExtensionAllowed)
                {
                    logger.LogDebug($"File extension is not allowed");
                    return Result.FromError($"File extension is not allowed");
                }

                var uploadFile = ConvertStreamToString(fileContent);

                if (String.IsNullOrEmpty(uploadFile.Trim()))
                {
                    logger.LogDebug($"Corrupted file");
                    return Result.FromError($"Corrupted file");
                }

                var schemaResource = EmbededResourceHelper.GetEmbeddedResource(EmbeddedResouceMetadata);

                var isShcemaValid = jsonSchemValidation.IsSchemaValid(schemaResource, uploadFile);
                if (!isShcemaValid)
                {
                    logger.LogDebug($"Invalid JSON schema");
                    return Result.FromError($"Invalid JSON schema");
                }

                var clinicalTrial = materializeHandler.Materialized(uploadFile);

                if (clinicalTrial.IsNone)
                {
                    return Result.FromError("Invalid JSON structure.");
                }

                var trial = processingHandler.Process(clinicalTrial.Value);

                await clinicalTrialsRepository.InsertAsync(trial);

                return Result.FromSuccess();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ConvertStreamToString(Stream fileContent)
        {
            using var reader = new StreamReader(fileContent);
            return reader.ReadToEnd();
        }
    }
}
