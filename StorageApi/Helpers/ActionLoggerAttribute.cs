using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace StorageApi.Helpers
{
    public class ActionLoggerAttribute : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            using StreamReader requestReader = new StreamReader(request.Body);
            string requestBody = await requestReader.ReadToEndAsync();
            object resultBody = "";
            if (context.Result is JsonResult data) resultBody = data.Value;
            Log.Information("Request: path='{@path}' body='{@body}' query='{@query}'\nResponse: body='{@resultBody}'", request.Path.Value, requestBody, request.QueryString.Value, resultBody);

            await base.OnResultExecutionAsync(context, next);
        }
    }
}
