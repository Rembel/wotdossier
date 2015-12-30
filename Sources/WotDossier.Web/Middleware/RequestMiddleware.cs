using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace WotDossier.Web.MiddleWare
{
    public class RequestMiddleware
    {
        RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            //var sw = new Stopwatch();
            //sw.Start();

            //using (var memoryStream = new MemoryStream())
            //{
            //    var bodyStream = context.Response.Body;
            //    context.Response.Body = memoryStream;
            //var isHtml = context.Response.ContentType?.ToLower().Contains("json");

            //if (isHtml != null && isHtml.Value)
            //{
            //    var bodyStream = context.Request.Body;

            //    using (var streamReader = new StreamReader(bodyStream))
            //    {
            //        var responseBody = await streamReader.ReadToEndAsync();
            //    }
            //}

            await _next(context);

            //    var isHtml = context.Response.ContentType?.ToLower().Contains("text/html");
            //    if (context.Response.StatusCode == 200 && isHtml.GetValueOrDefault())
            //    {
            //        {
            //            memoryStream.Seek(0, SeekOrigin.Begin);
            //            using (var streamReader = new StreamReader(memoryStream))
            //            {
            //                var responseBody = await streamReader.ReadToEndAsync();
            //                var newFooter = @"<footer><div id=""process"">Page processed in {0} milliseconds.</div>";
            //                responseBody = responseBody.Replace("<footer>", string.Format(newFooter, sw.ElapsedMilliseconds));
            //                context.Response.Headers.Add("X-ElapsedTime", new[] { sw.ElapsedMilliseconds.ToString() });
            //                using (var amendedBody = new MemoryStream())
            //                using (var streamWriter = new StreamWriter(amendedBody))
            //                {
            //                    streamWriter.Write(responseBody);
            //                    amendedBody.Seek(0, SeekOrigin.Begin);
            //                    await amendedBody.CopyToAsync(bodyStream);
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
