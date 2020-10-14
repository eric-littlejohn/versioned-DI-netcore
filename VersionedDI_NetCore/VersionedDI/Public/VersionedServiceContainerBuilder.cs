using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using VersionedDI.Internal;

namespace VersionedDI
{
    public sealed class VersionedServiceContainerBuilder
    {
        public static VersionedServiceContainerBuilder Create()
            => new VersionedServiceContainerBuilder(null, null);

        public static VersionedServiceContainerBuilder Create(IServiceCollection services)
            => new VersionedServiceContainerBuilder(services, null);

        public static VersionedServiceContainerBuilder Create(IServiceCollection services, IServiceAssemblyScanner assemblyScanner)
            => new VersionedServiceContainerBuilder(services, assemblyScanner);

        private readonly IServiceAssemblyScanner _assemblyScanner;
        private IServiceCollection _services;

        internal VersionedServiceContainerBuilder(IServiceCollection services, IServiceAssemblyScanner assemblyScanner)
        {
            _services = services ?? new ServiceCollection();
            _assemblyScanner = assemblyScanner ?? new VersionedServiceAssemblyScanner();
        }

        public VersionedServiceContainerBuilder AddVersionedServicesFromAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var servicesFromAssembly = _assemblyScanner.ScanAssemblyForDecoratedVersionedServices(assembly, ref _services);
                foreach (var serviceFromAssembly in servicesFromAssembly)
                {
                    _services.Add(serviceFromAssembly);
                }
            }

            return this;
        }

        public ServiceContainer Build => new ServiceContainer(_services);
    }
}
