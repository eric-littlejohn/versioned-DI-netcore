using System;

namespace VersionedDI
{
    /// <summary>
    /// Decorates a parameter to be injected with a service that matches the version provided.
    /// </summary>
    /// <example>
    ///     [VersionedService(typeof(ISampleSerivce), "1.0")]
    ///     [VersionedService(typeof(ISampleSerivce), "1.1")]
    ///     public class SampleSerivce_V1
    ///     { 
    ///         public SampleSerivce_V1([VersionedServiceParameter("1.0")] IOtherService serviceDependency)]
    ///     }
    ///     
    ///     When the SampleSerivce_V1 is requested and being created, it will read the constructor arguments
    ///     and if an argument is decoreated with the VersionedServiceParameter, it will try to inject the service decorated
    ///     with the specified version. If there isnt a service with the version specified, an exception will be thrown.
    /// </example>
    /// <example>
    ///     [VersionedService(typeof(ISampleSerivce), "1.0")]
    ///     [VersionedService(typeof(ISampleSerivce), "1.1")]
    ///     public class SampleSerivce_V1 
    ///     { 
    ///         public SampleSerivce_V1([VersionedServiceParameter("1.0", Required = false)] IOtherService serviceDependency)]
    ///     }
    ///     
    ///     When the SampleSerivce_V1 is requested and being created, it will read the constructor arguments
    ///     and if an argument is decoreated with the VersionedServiceParameter, it will try to inject the service decorated
    ///     with the specified version. If there isnt a service with the version specified and required is set to false, 
    ///     no exception will be thrown and null will be returned.
    /// </example>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class VersionedServiceParameterAttribute : Attribute
    {
        /// <summary>
        /// The version the decorated service supports.
        /// </summary>
        public string SupportedVersion { get; }

        /// <summary>
        /// Indicates whether the parameter decorated is required to be injected with a value.
        /// </summary>
        public bool Required { get; set; } 

        public VersionedServiceParameterAttribute(string supportedVersion)
        { 
            SupportedVersion = supportedVersion;
        }
    }
}
