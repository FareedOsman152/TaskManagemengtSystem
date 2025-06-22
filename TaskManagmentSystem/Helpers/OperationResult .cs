namespace TaskManagmentSystem.Helpers
{
    public class OperationResult
    {
        public bool Succeeded{ get; set; }
        public string ErrorMessage{ get; set; }
        public static OperationResult Success ()
            => new OperationResult { Succeeded = true };
        public static OperationResult Failure(string errorMessage)
        => new OperationResult {Succeeded = false, ErrorMessage = errorMessage };
    }
}
