using System;

namespace VersionedDI
{
    /// <summary>
    /// Decorates a class to be identified as a "Versioned" service.
    /// </summary>
    /// <example>
    ///     [VersionedService(typeof(ISampleService), "1.0")]
    ///     [VersionedService(typeof(ISampleService), "1.1")]
    ///     public class SampleService_V1 { ... }
    ///     
    ///     The above code would register this class as the type to be returned when requesting
    ///     an "ISampleService" service instance for version 1.0 and 1.1 manifests.
    /// </example>
    /// <example>
    ///     [VersionedService(typeof(ISampleService), "1.0", true)]
    ///     [VersionedService(typeof(ISampleService), "1.1", true)]
    ///     public class SampleService_V1 { ... }
    ///     
    ///     The above code would register this class as the type to be returned when requesting
    ///     an "ISampleService" service instance for version 1.0 and 1.1 manifests.
    ///     It will return the same instance per version since the IsSingleton flag is set to true.
    ///     As in, when version 1.0 is requested it will return the same instance registered for 1.0. If version 1.1 is requested it will return the same instance registered for version 1.1.
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionedServiceAttribute : Attribute
    {
        /// <summary>
        /// Indicates whether or not the decorated class should be registered as a singleton.
        /// </summary>
        public bool IsSingleton { get; }

        /// <summary>
        /// The type the decorated class should be registered under.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// The version the decorated service supports.
        /// </summary>
        public string SupportedVersion { get; }

        public VersionedServiceAttribute(Type serviceType, string supportedVersion)
            : this(serviceType, false, supportedVersion)
        { }

        public VersionedServiceAttribute(Type serviceType, bool isSingleton, string supportedVersion)
        {
            IsSingleton = isSingleton;
            ServiceType = serviceType;
            SupportedVersion = supportedVersion;
        }
    }
}
