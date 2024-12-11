
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ClinicalTrials.Application.Validations
{
    internal class JsonSchemValidation : IJsonSchemValidation
    {
        public bool IsSchemaValid(string resourceSchema, string documentJson)
        {
            JSchema schema = JSchema.Parse(resourceSchema);
            var document = JObject.Parse(documentJson);

            var errors = new List<string>();

            bool isValid = document.IsValid(schema, out IList<string> validationErrors);

            if (!isValid)
            {
                foreach (var error in validationErrors)
                {
                    errors.Add(error);
                }
            }

            return isValid;
        }
    }
}
