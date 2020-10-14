using System;

namespace VersionedDI
{
    /// <summary>
    /// Interfaces declaration for an <see cref="IServiceProvider"/> that supports retrieved versioned services.
    /// </summary>
    public interface IVersionedServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Gets an instance of the concrete implementation for the requested service that supports the provided version.
        /// </summary>
        /// <param name="version">The version that the service is to support.</param>
        /// <param name="requestedService">The requested service type.</param>
        /// <returns>An instance of the requested service that can support the version specified.</returns>
        object GetService(Type requestedService, string version);
    }
}
