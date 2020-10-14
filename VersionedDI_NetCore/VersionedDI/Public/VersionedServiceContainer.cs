using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VersionedDI.Internal;

namespace VersionedDI
{
    public sealed class ServiceContainer
    {
        /// <summary>
        /// The services stored in the container instance.
        /// </summary>
        public IServiceProvider Services { get; }

        internal ServiceContainer(IEnumerable<ServiceDescriptor> services = null)
        {
            Services = new VersionedServiceProvider(services ?? new ServiceCollection());
        }
    }
}
