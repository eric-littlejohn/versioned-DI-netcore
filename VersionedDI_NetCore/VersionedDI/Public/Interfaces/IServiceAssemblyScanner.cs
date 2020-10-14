using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VersionedDI
{
    /// <summary>
    /// Interface declaration for a scanner that will scan an <see cref="Assembly"/> to retrieve services.
    /// </summary>
    public interface IServiceAssemblyScanner
    {
        /// <summary>
        /// Scans the provided <see cref="Assembly"/> for decorated versioned services.
        /// </summary>
        /// <param name="assembly">The Assembly to scan.</param>
        /// <returns>The <see cref="ServiceDescriptor"/>s for the decorated services found.</returns>
        IEnumerable<ServiceDescriptor> ScanAssemblyForDecoratedVersionedServices(Assembly assembly, ref IEnumerable<ServiceDescriptor> existingServices);
    }
}
