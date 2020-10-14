using Microsoft.Extensions.DependencyInjection;
using System;

namespace VersionedDI
{
    public interface IVersionedServiceActivator
    {
        object CreateServiceInstance(IServiceProvider provider, ServiceDescriptor serviceDescriptor);
    }
}
