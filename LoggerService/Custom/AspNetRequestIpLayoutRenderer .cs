using Microsoft.AspNetCore.Http;
using NLog;
using NLog.LayoutRenderers;
using System;
using System.Text;

namespace LoggerService.Custom
{
    [LayoutRenderer("aspnet-request-ip")]
    public class AspNetRequestIpLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = new HttpContextAccessor();
            if (httpContext == null)
            {
                builder.Append("Dont allowed");
            }
            builder.Append(httpContext.HttpContext.Connection.RemoteIpAddress.ToString());
        }
    }
}