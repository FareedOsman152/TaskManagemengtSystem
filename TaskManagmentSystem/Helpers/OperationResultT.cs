namespace TaskManagmentSystem.Helpers
{
    public class OperationResult<T>
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
        public static OperationResult<T> Success(T data)
            => new OperationResult<T> { Succeeded = true , Data = data};
        public static OperationResult<T> Failure(string errorMessage)
        => new OperationResult<T> { Succeeded = false, ErrorMessage = errorMessage };
    }
}
