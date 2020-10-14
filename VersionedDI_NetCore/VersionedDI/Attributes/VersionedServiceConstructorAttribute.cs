using System;

namespace VersionedDI
{
    /// <summary>
    /// Decorates a constructor to be used when instantiating the class as a "Versioned" service.
    /// </summary>
    /// <example>
    ///     [VersionedService(typeof(ISampleService), "1.0", "1.1")]
    ///     public class SampleService_V1 
    ///     {
    ///         public SampleService_V1(...) { ... }
    ///         
    ///         [VersionedServiceConstructor]
    ///         public SampleService_V1( ... ) { ... }
    ///     }
    ///     
    ///     The above code would use the second constructor when trying to instatiate the service as a versioned service.
    ///     It would not use the first constructor since there is at least one constructor decorated with the VersionedSerivceConstructor attribute.
    /// </example>
    /// <example>
    ///     [VersionedService(typeof(ISampleService), "1.0", "1.1")]
    ///     public class SampleService_V1 
    ///     {
    ///         public SampleService_V1(...) { ... }
    ///     
    ///         [VersionedServiceConstructor]
    ///         public SampleService_V1(...) { ... }
    ///         
    ///         [VersionedServiceConstructor(0)]
    ///         public SampleService_V1( ... ) { ... }
    ///     }
    ///     
    ///     The above code would use the try to use constructors 2 and 3 when trying to instatiate the service as a versioned service.
    ///     It would not use the first constructor since there is at least one constructor decorated with the VersionedSerivceConstructor attribute so it only used decorated constructors.
    ///     The system would try to use constructor 3 first since it has the highest priority. Since constructor 2 doesnt specify a priority, it defaults to the lowest.
    /// </example>
    /// <example>
    ///     [VersionedService(typeof(ISampleService), "1.0", "1.1")]
    ///     public class SampleService_V1 
    ///     {
    ///         public SampleService_V1(...) { ... }
    ///         
    ///         public SampleService_V1( ... ) { ... }
    ///     }
    ///     
    ///     The above code would try to use both constructors when trying to instatiate the service as a versioned service.
    ///     This is because there isnt any constructors decorated with the VersionedSerivceConstructor attribute.
    /// </example>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class VersionedServiceConstructorAttribute : Attribute
    {
        /// <summary>
        /// The priority of the decorated constructor. The lower the priority, the higher it is. 
        /// </summary>
        public uint Priority { get; }

        public VersionedServiceConstructorAttribute(uint priority = uint.MaxValue)
        {
            Priority = priority;
        }
    }
}
