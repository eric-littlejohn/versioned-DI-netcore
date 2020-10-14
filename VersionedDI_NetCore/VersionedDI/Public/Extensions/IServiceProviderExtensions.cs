using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VersionedDI.Internal;

namespace VersionedDI
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Gets an instance of the concrete implementation for the requested service that supports the provided version.
        /// Will return the default of <typeparamref name="TService"/> if no service is found.
        /// </summary>
        /// <typeparam name="TService">The type of the service to request.</typeparam>
        /// <param name="version">The version that the service is to support.</param>
        /// <returns>An instance of the requested service that can support the version specified.</returns>
        public static TService GetService<TService>(this IServiceProvider provider, string version)
        {
            if (provider is VersionedServiceProvider versionedProvider)
            {
                return (TService)versionedProvider.GetService(typeof(TService), version);
            }

            throw new NotSupportedException("The provider does not support versioned services");
        }

        /// <summary>
        /// Gets an instance of the concrete implementation for the requested service that supports the provided version.
        /// Will throw if a service is not found.
        /// </summary>
        /// <param name="version">The version that the service is to support.</param>
        /// <param name="requestedService">The requested service type.</param>
        /// <returns>An instance of the requested service that can support the version specified.</returns>
        public static TService GetRequiredService<TService>(this IServiceProvider provider, string version)
            => GetService<TService>(provider, version) ?? throw new Exception($"The service of type {typeof(TService).FullName} cannot be null.");
    }
}
