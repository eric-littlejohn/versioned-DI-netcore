using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VersionedDI.Internal
{
    internal class VersionedServiceAssemblyScanner : IServiceAssemblyScanner
    {
        public IEnumerable<ServiceDescriptor> ScanAssemblyForDecoratedVersionedServices(Assembly assembly, ref IServiceCollection existingServices)
        {
            var newServices = new List<ServiceDescriptor>();
            var versionedServiceTypesInAssembly = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<VersionedServiceAttribute>() != null);

            foreach (var versionedServiceType in versionedServiceTypesInAssembly)
            {
                var injectableServiceAttr = versionedServiceType.GetCustomAttribute<VersionedServiceAttribute>();
                var supportedVersion = injectableServiceAttr.SupportedVersion;
                var existingService = existingServices
                    .Concat(newServices)
                    .FirstOrDefault(d => d is VersionedServiceDescriptor &&
                        d.ServiceType == injectableServiceAttr.ServiceType &&
                        String.Equals((d as VersionedServiceDescriptor).SupportedVersion, supportedVersion, StringComparison.OrdinalIgnoreCase));

                if (existingService != null)
                {
                    throw new InvalidOperationException(String.Format("A service for version {0} under type {1} has already been registered. Already Registered Type: {2}, Type trying to register: {3}",
                        supportedVersion, injectableServiceAttr.ServiceType.FullName, existingService.ImplementationType.FullName, versionedServiceType.FullName));
                }
                else
                {
                    var serviceMetadata = new VersionedServiceDescriptor(injectableServiceAttr.ServiceType, versionedServiceType, 
                        injectableServiceAttr.IsSingleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient, supportedVersion);

                    newServices.Add(serviceMetadata);
                }

            }

            return newServices;
        }
    }
}
