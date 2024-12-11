namespace ClinicalTrials.Application.Helpers
{
    public readonly record struct Result
    {
        public static Result FromError(string? errorMessage)
        {
            return new Result(false, errorMessage);
        }

        public static Result FromSuccess()
        {
            return new Result(true);
        }

        private Result(bool isSuccess, string? errorMessage = null)
        {
            ErrorMessage = errorMessage;
            IsSuccess = isSuccess;
        }

        public string? ErrorMessage { get; }

        public bool IsSuccess { get; }
    }
}
