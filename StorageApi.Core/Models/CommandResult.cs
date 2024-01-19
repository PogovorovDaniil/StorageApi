
namespace StorageApi.Core.Models
{
    public class CommandResult
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CommandResult<T> : CommandResult
    {
        public new T Result
        {
            get { return (T)base.Result; }
            set { base.Result = value; }
        }
    }
}
