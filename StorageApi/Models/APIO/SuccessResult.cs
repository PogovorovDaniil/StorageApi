namespace StorageApi.Models.APIO
{
    public class SuccessResult
    {
        public string Message { get; set; }
        public SuccessResult(string message)
        {
            Message = message;
        }
    }
}
