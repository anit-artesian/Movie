using System.Text;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.LayoutRenderers;

[LayoutRenderer("basedir")]
public class AspNetCoreBaseDirLayoutRenderer : LayoutRenderer
{

  
    public static IHostingEnvironment Env {get;set;}

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        builder.Append(Env.ContentRootPath);
    }
}