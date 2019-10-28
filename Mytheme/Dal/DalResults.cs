namespace Mytheme.Dal
{
    public class DalResult 
    {
        public DalStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => Status == DalStatus.Success;

        public DalResult()
        {
            
        }

        public DalResult(DalStatus status, string message = "")
        {
            Status = status;
            Message = message;
        }
    }

    public class DalResult<T> : DalResult
    {
        public T Result { get; set; }

        public DalResult()
        {
            
        }

        public DalResult(DalStatus status, T result, string message = "")
        {
            Status = status;
            Result = result;
            Message = message;
        }
    }

    public enum DalStatus
    {
        Success,
        ConstraintViolation,
        Unknown
    }
}
