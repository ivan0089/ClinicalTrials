namespace ClinicalTrials.Application.Validations
{
    public interface IJsonSchemValidation
    {
        bool IsSchemaValid(string resourceSchema, string file);
    }
}