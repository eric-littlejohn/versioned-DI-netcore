using Microsoft.Extensions.DependencyInjection;
using System;

namespace VersionedDI.Internal
{
    internal sealed class VersionedServiceDescriptor : ServiceDescriptor
    {            
        internal string SupportedVersion { get; }

        internal VersionedServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime, string version)
            : base(serviceType, implementationType, lifetime)
        {
            SupportedVersion = version;
        }
    }
}
