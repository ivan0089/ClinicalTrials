using ClinicalTrials.Application.Validations;
using FluentAssertions;

namespace ClinicalTrials.Application.Tests.Validations
{
    [TestClass]
    public class JsonSchemValidationTests
    {
        private JsonSchemValidation? _testee;

        [TestInitialize]
        public void TestInitialize()
        {
            _testee = new JsonSchemValidation();
        }

        [TestMethod]
        public void IsSchemaValid_ShouldReturnTrue_ForValidSchemaAndDocument()
        {
            // Arrange
            var schema = """
            {
                "$schema": "http://json-schema.org/draft-07/schema#",
                "title": "ClinicalTrialMetadata",
                "type": "object",
                "properties": {
                    "trialId": {
                        "type": "string"
                    },
                    "title": {
                        "type": "string"
                    },
                    "startDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "endDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "participants": {
                        "type": "integer",
                        "minimum": 1
                    },
                    "status": {
                        "type": "string",
                        "enum": [
                            "Not Started",
                            "Ongoing",
                            "Completed"
                        ]
                    }
                },
                "required": [
                    "trialId",
                    "title",
                    "startDate",
                    "status"
                ],
                "additionalProperties": false
            }
            """;

            var document = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 12,
                    "status": "Completed"
                }
                """;

            // Act
            var result = _testee!.IsSchemaValid(schema, document);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsSchemaValid_ShouldReturnFalse_ForInvalidSchemaAndDocument_MissingRequiredField()
        {
            var schema = """
            {
                "$schema": "http://json-schema.org/draft-07/schema#",
                "title": "ClinicalTrialMetadata",
                "type": "object",
                "properties": {
                    "trialId": {
                        "type": "string"
                    },
                    "title": {
                        "type": "string"
                    },
                    "startDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "endDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "participants": {
                        "type": "integer",
                        "minimum": 1
                    },
                    "status": {
                        "type": "string",
                        "enum": [
                            "Not Started",
                            "Ongoing",
                            "Completed"
                        ]
                    }
                },
                "required": [
                    "trialId",
                    "title",
                    "startDate",
                    "status"
                ],
                "additionalProperties": false
            }
            """;

            var document = """
                {
                    "trialId": "12345",
                    "startDate": "2024-01-01",
                    "participants": 0,
                    "status": "Completed"
                }
                """;

            // Act
            var result = _testee!.IsSchemaValid(schema, document);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsSchemaValid_ShouldReturnFalse_ForMalformedDocument_MissingMinimum()
        {
            // Arrange
            var schema = """
            {
                "$schema": "http://json-schema.org/draft-07/schema#",
                "title": "ClinicalTrialMetadata",
                "type": "object",
                "properties": {
                    "trialId": {
                        "type": "string"
                    },
                    "title": {
                        "type": "string"
                    },
                    "startDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "endDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "participants": {
                        "type": "integer",
                        "minimum": 1
                    },
                    "status": {
                        "type": "string",
                        "enum": [
                            "Not Started",
                            "Ongoing",
                            "Completed"
                        ]
                    }
                },
                "required": [
                    "trialId",
                    "title",
                    "startDate",
                    "status"
                ],
                "additionalProperties": false
            }
            """;

            var document = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 0,
                    "status": "Completed"
                }
                """;

            // Act
            var result = _testee!.IsSchemaValid(schema, document);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsSchemaValid_ShouldReturnFalse_ForMalformedDocument_MissingEnum()
        {
            // Arrange
            var schema = """
            {
                "$schema": "http://json-schema.org/draft-07/schema#",
                "title": "ClinicalTrialMetadata",
                "type": "object",
                "properties": {
                    "trialId": {
                        "type": "string"
                    },
                    "title": {
                        "type": "string"
                    },
                    "startDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "endDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "participants": {
                        "type": "integer",
                        "minimum": 1
                    },
                    "status": {
                        "type": "string",
                        "enum": [
                            "Not Started",
                            "Ongoing",
                            "Completed"
                        ]
                    }
                },
                "required": [
                    "trialId",
                    "title",
                    "startDate",
                    "status"
                ],
                "additionalProperties": false
            }
            """;

            var document = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": 12,
                    "status": "Test"
                }
                """;

            // Act
            var result = _testee!.IsSchemaValid(schema, document);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsSchemaValid_ShouldReturnFalse_ForMalformedDocument()
        {
            // Arrange
            var schema = """
            {
                "$schema": "http://json-schema.org/draft-07/schema#",
                "title": "ClinicalTrialMetadata",
                "type": "object",
                "properties": {
                    "trialId": {
                        "type": "string"
                    },
                    "title": {
                        "type": "string"
                    },
                    "startDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "endDate": {
                        "type": "string",
                        "format": "date"
                    },
                    "participants": {
                        "type": "integer",
                        "minimum": 1
                    },
                    "status": {
                        "type": "string",
                        "enum": [
                            "Not Started",
                            "Ongoing",
                            "Completed"
                        ]
                    }
                },
                "required": [
                    "trialId",
                    "title",
                    "startDate",
                    "status"
                ],
                "additionalProperties": false
            }
            """;

            var document = """
                {
                    "trialId": "12345",
                    "title": "Trial A",
                    "startDate": "2024-01-01",
                    "participants": "test",
                    "status": "Completed"
                }
                """;

            // Act
            var result = _testee!.IsSchemaValid(schema, document);

            // Assert
            result.Should().BeFalse();
        }
    }
}
