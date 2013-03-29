using System;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using UnityConfiguration;

namespace Uncas.BuildPipeline
{
    [CLSCompliant(false)]
    public class SharedRegistry : UnityRegistry
    {
        public SharedRegistry()
        {
            Assembly assembly = typeof (SharedRegistry).Assembly;
            Scan(scan =>
                {
                    scan.Assembly(assembly);
                    scan.WithNamingConvention();
                });
            Register<IServiceLocator, UnityServiceLocator>();
        }
    }
}