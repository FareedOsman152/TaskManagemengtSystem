namespace TaskManagmentSystem.Helpers
{
    public static class OperationResultExtensions
    {
        public static OperationResult<T> Ensure<T>(
            this OperationResult<T> result,
            Func<T,bool> validation,
            string errorMessage)
        {
            if(result.Succeeded && !validation(result.Data))
                return OperationResult<T>.Failure(errorMessage);
            return result;
        }
    }
}
