
using Carter;
using ClinicalTrials.Application.ClinicalTrials;
using ClinicalTrials.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace ClinicalTrials.Api.Features
{
    public class ClinicalTrialsCarterModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var clinicalTrialsGroup = app.MapGroup("/v1/trials")
                               .WithTags("Clinical trilas endpoints");

            clinicalTrialsGroup.MapGet("/{id}", GetClinicalTrialsById)
                            .WithName(nameof(GetClinicalTrialsById))
                            .WithOpenApi(operation =>
                            {
                                operation.Summary = "Obtains clinical trail information";
                                operation.Parameters = new List<OpenApiParameter>
                                {
                                    new OpenApiParameter
                                    {
                                        Name = "id",
                                        In = ParameterLocation.Path,
                                        Description = "The Id of clinical trial",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "string"
                                        }
                                    }
                                };
                                operation.Responses = new OpenApiResponses
                                {
                                    {
                                        "200", new OpenApiResponse
                                        {
                                            Description = "Clinical trial information retreive successfully"
                                        }
                                    },
                                    {
                                        "400", new OpenApiResponse
                                        {
                                            Description = "Invalid input data"
                                        }
                                    }
                                };
                                return operation;
                            });

            clinicalTrialsGroup.MapGet("", GetClinicalTrialsByFilterOptions)
                            .WithName(nameof(GetClinicalTrialsByFilterOptions))
                            .WithOpenApi(operation =>
                            {
                                operation.Summary = "Filter clinical trail information";
                                operation.Parameters = new List<OpenApiParameter>
                                {
                                    // Status filter
                                    new OpenApiParameter
                                    {
                                        Name = "status",
                                        In = ParameterLocation.Query,
                                        Description = "The status of the clinical trial (e.g., 'Started', 'Ongoing', 'Completed')",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "string"
                                        }
                                    },
            
                                    // Participants filter
                                    new OpenApiParameter
                                    {
                                        Name = "participants",
                                        In = ParameterLocation.Query,
                                        Description = "The number of participants in the clinical trial",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "integer",
                                            Format = "int32"
                                        }
                                    },
            
                                    // ParticipantOperator filter (lt or gt)
                                    new OpenApiParameter
                                    {
                                        Name = "participantOperator",
                                        In = ParameterLocation.Query,
                                        Description = "Operator to filter participants: 'lt' (less than) or 'gt' (greater than)",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "string",
                                            Enum = new List<IOpenApiAny>
                                            {
                                                new OpenApiString("lt"),  // less than
                                                new OpenApiString("gt")   // greater than
                                            }
                                        }
                                    },

                                    // StartDate filter
                                    new OpenApiParameter
                                    {
                                        Name = "startDate",
                                        In = ParameterLocation.Query,
                                        Description = "The start date of the clinical trial",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "string",
                                            Format = "date"
                                        }
                                    },

                                    // StartDateOperator filter (lt or gt)
                                    new OpenApiParameter
                                    {
                                        Name = "startDateOperator",
                                        In = ParameterLocation.Query,
                                        Description = "Operator to filter start date: 'lt' (less than) or 'gt' (greater than)",
                                        Required = false,
                                        Schema = new OpenApiSchema
                                        {
                                            Type = "string",
                                            Enum = new List<IOpenApiAny>
                                            {
                                                new OpenApiString("lt"),  // less than
                                                new OpenApiString("gt")   // greater than
                                            }
                                        }
                                    }
                                };
                                operation.Responses = new OpenApiResponses
                                {
                                    {
                                        "200", new OpenApiResponse
                                        {
                                            Description = "Clinical trial information retreive successfully for filter options"
                                        }
                                    },
                                    {
                                        "400", new OpenApiResponse
                                        {
                                            Description = "Invalid input data"
                                        }
                                    }
                                };
                                return operation;
                            });

            clinicalTrialsGroup.MapPost("/upload", UploadClinicalTrials)
                .WithName(nameof(UploadClinicalTrials))
                .WithOpenApi(operation =>
                {
                    operation.Summary = "Save clinical trail information";
                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                "multipart/form-data", new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Type = "object",
                                        Properties = new Dictionary<string, OpenApiSchema>
                                        {
                                            ["file"] = new OpenApiSchema
                                            {
                                                Type = "string",
                                                Format = "binary",
                                                Description = "The JSON file containing clinical trial data"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };
                    operation.Responses = new OpenApiResponses
                    {
                        {
                            "204", new OpenApiResponse
                            {
                                Description = "Clinical trial information saved successfully"
                            }
                        },
                        {
                            "400", new OpenApiResponse
                            {
                                Description = "Invalid input data"
                            }
                        }
                    };
                    return operation;
                });
        }

        internal async ValueTask<Results<BadRequest<string>, Ok<ClinicalTrial>>> GetClinicalTrialsById(string id, IClinicalTrialsServices clinicalTrialsServices,  ILogger<ClinicalTrialsCarterModule> logger, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"{nameof(GetClinicalTrialsByFilterOptions)}: Retrieving clinical trial by {nameof(id)} = {id}");

                var result = await clinicalTrialsServices.GetTrialByIdAsync(id);

                return TypedResults.Ok(result);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"{nameof(GetClinicalTrialsById)}: Error while retrieving trial information for {nameof(id)} = {id}.");
                return TypedResults.BadRequest("Error while retrieving clinical trial information.");
            }
        }

        internal async ValueTask<Results<BadRequest<string>, Ok<IEnumerable<ClinicalTrial>>>> GetClinicalTrialsByFilterOptions([FromQuery] string? status,
            [FromQuery] int? participants,
            [FromQuery] string? participantOperator,
            [FromQuery] DateTime? startDate,
            [FromQuery] string? startDateOperator, 
            IClinicalTrialsServices clinicalTrialsServices,  
            ILogger<ClinicalTrialsCarterModule> logger, 
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"{nameof(GetClinicalTrialsByFilterOptions)}: Retrieving clinical trial by filter options");

                var filter = new ClinicalTrialFilter
                {
                    Status = status,
                    Participants = participants,
                    ParticipantOperator = participantOperator,
                    StartDate = startDate,
                    StartDateOperator = startDateOperator
                };

                var result = await clinicalTrialsServices.GetClinicalTrialsByFilterOptionsAsync(filter);

                return TypedResults.Ok(result);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"{nameof(GetClinicalTrialsByFilterOptions)}: Error while retreiving filtered clinical trial information.");
                return TypedResults.BadRequest("Error while retreiving filtered clinical trial information.");
            }
        }

        internal async  ValueTask<Results<BadRequest<string>, NoContent>> UploadClinicalTrials(HttpRequest request,  IClinicalTrialsServices clinicalTrialsServices, ILogger<ClinicalTrialsCarterModule> logger, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"{nameof(UploadClinicalTrials)}: Pocessing clinical trial.");

                if (!request.HasFormContentType || request.Form.Files.Count == 0)
                {
                    return TypedResults.BadRequest("Invalid request. Ensure a file is uploaded.");
                }

                var file = request.Form.Files[0];
                var fileName = file.FileName;
                var fileContent = file.OpenReadStream();

                var result = await clinicalTrialsServices.HandleClinicalTrialAsync(fileName, fileContent);
                return result.IsSuccess
                    ? TypedResults.NoContent()
                    : TypedResults.BadRequest(result.ErrorMessage);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"{nameof(UploadClinicalTrials)}: Error while processing clinical trial information.");
                return TypedResults.BadRequest("Error while processing clinical trial information.");
            }
        }
    }
}
