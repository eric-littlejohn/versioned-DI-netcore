using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VersionedDI.Internal
{
    internal sealed class VersionedServiceProvider : IVersionedServiceProvider
    {
        private readonly IVersionedServiceActivator _serviceActivator;

        private readonly IServiceCache _serviceCache;

        private readonly IEnumerable<ServiceDescriptor> _services;

        internal VersionedServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors)
            : this(serviceDescriptors, new ServiceActivator(), new ServiceCache())
        {
        }

        internal VersionedServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors, IVersionedServiceActivator serviceActivator, IServiceCache serviceCache)
        {
            _services = serviceDescriptors ?? Enumerable.Empty<ServiceDescriptor>();
            _serviceActivator = serviceActivator ?? throw new ArgumentNullException(nameof(serviceActivator), "Service activator cannot be null");
            _serviceCache = serviceCache ?? throw new ArgumentNullException(nameof(serviceCache), "Service cache cannot be null");
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            //Search the service collection for the first service that matches the provided type AND isnt a versioned service.
            var serviceDescription = _services
                .FirstOrDefault(s => !(s is VersionedServiceDescriptor) && s.ServiceType == serviceType);

            if (serviceDescription == null)
            {
                throw new KeyNotFoundException($"No service registered for type {serviceType.FullName}.");
            }

            return GetService(serviceDescription, serviceDescription.ServiceType) ??
                throw new InvalidOperationException($"Unable to create service instance for type {serviceType.FullName}.");
        }

        /// <inheritdoc />
        public object GetService(Type requestedService, string version)
        {
            var serviceDescription = _services
                .Where(s => s is VersionedServiceDescriptor)
                .Select(s => s as VersionedServiceDescriptor)
                .FirstOrDefault(s => s.ServiceType == requestedService &&
                    String.Equals(s.SupportedVersion, version, StringComparison.OrdinalIgnoreCase));

            if (serviceDescription == null)
            {
                throw new KeyNotFoundException($"No service registered for type {requestedService.FullName} that supports version {version}");
            }

            return GetService(serviceDescription, new Tuple<Type, string>(serviceDescription.ServiceType, version)) ?? 
                throw new InvalidOperationException($"Unable to create service instance for type {requestedService.FullName} that supports version {version}");
        }

        private object GetService(ServiceDescriptor descriptor, object cacheKey)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton && _serviceCache[cacheKey] != null)
            {
                return _serviceCache[cacheKey];
            }

            var instance = _serviceActivator.CreateServiceInstance(this, descriptor);

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                _serviceCache.Add(cacheKey, instance);
            }

            return instance;
        }
    }
}
