namespace StorageApi.Models.TemplateResult
{
    public class ExceptionResult
    {
        public string ErrorText { get; set; }
        public ExceptionResult(string errorText)
        {
            ErrorText = errorText;
        }
    }
}
