using System;
using UnityConfiguration;

namespace Uncas.BuildPipeline.Web.Configuration
{
    [CLSCompliant(false)]
    public class WebRegistry : UnityRegistry
    {
        public WebRegistry()
        {
            Scan(scan =>
                {
                    scan.Assembly(typeof (WebRegistry).Assembly);
                    scan.With<NamingConvention>();
                });
        }
    }
}