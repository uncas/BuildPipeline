using System;
using UnityConfiguration;

namespace Uncas.BuildPipeline
{
    [CLSCompliant(false)]
    public class SharedRegistry : UnityRegistry
    {
        public SharedRegistry()
        {
            Scan(scan =>
                {
                    scan.Assembly(typeof (SharedRegistry).Assembly);
                    scan.With<NamingConvention>();
                });
        }
    }
}