namespace StorageApi.Core.Models.TemplateResult
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
