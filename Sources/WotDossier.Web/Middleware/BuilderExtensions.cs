using Microsoft.AspNet.Builder;
using WotDossier.Web.MiddleWare;

namespace WotDossier.Web.Middleware
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestMiddleware>();
        }
    }
}
