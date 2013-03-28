using System;
using UnityConfiguration;

namespace Uncas.BuildPipeline.WindowsService
{
    [CLSCompliant(false)]
    public class ServiceRegistry : UnityRegistry
    {
        public ServiceRegistry()
        {
            Scan(scan =>
                {
                    scan.Assembly(typeof (ServiceRegistry).Assembly);
                    scan.With<NamingConvention>();
                });
        }
    }
}