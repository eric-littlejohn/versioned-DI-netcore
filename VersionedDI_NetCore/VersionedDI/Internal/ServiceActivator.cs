using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VersionedDI.Internal
{
    internal sealed class ServiceActivator : IVersionedServiceActivator
    {
        public object CreateServiceInstance(IServiceProvider provider, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton && serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance;
            }

            object instance = null;

            IEnumerable<ConstructorInfo> implementationConstructors = serviceDescriptor switch
            {
                VersionedServiceDescriptor vServiceDescriptor => GetConstructorsForVersionedService(vServiceDescriptor.ImplementationType),
                _ => GetConstructorsForService(serviceDescriptor.ImplementationType)
            };

            foreach (var ctorInfo in implementationConstructors)
            {
                try
                {
                    var ctorParams = ctorInfo.GetParameters();
                    object[] ctorArgs = new object[ctorParams.Length];
                    bool canResolve = true;
                    for (int i = 0; i < ctorParams.Length; i++)
                    {
                        var ctorParam = ctorParams[i];

                        object paramInstance;

                        var versionedParamDecorator = ctorParams[i].GetCustomAttribute<VersionedServiceParameterAttribute>();

                        if (versionedParamDecorator != null)
                        {
                            if (!(provider is IVersionedServiceProvider versionedProvider))
                            {
                                throw new InvalidOperationException("Service provider does not support retrieving versioned services.");
                            }

                            //See if there is a registered service that matches the same version as the described service that can be resolved to that param type.
                            paramInstance = versionedProvider.GetService(ctorParam.ParameterType, versionedParamDecorator.RequestedVersion);
                        }
                        else
                        {
                            //Try to find a non-versioned service that can resolve to the parameter type
                            paramInstance = provider.GetService(ctorParam.ParameterType);
                        }

                        //If the parameter still cannot be resolved by this point, 
                        //The constructor cannot be auto resolved.
                        if (paramInstance == null)
                        {
                            canResolve = false;
                            break;
                        }

                        //Set the param instance in the in the ctorArgs array
                        ctorArgs[i] = paramInstance;
                    }

                    if (canResolve)
                    {
                        instance = Activator.CreateInstance(serviceDescriptor.ImplementationType, ctorArgs);
                        break;
                    }
                }
                catch (StackOverflowException)
                {
                    continue;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //If an instance still hasnt been found by this point, the service is missing dependencies
            if (instance == null)
            {
                throw new InvalidOperationException($"Unable to create an instance of type {serviceDescriptor.ImplementationType.FullName}. Ensure all dependencies are registered and no circular references are set.");
            }

            return instance;
        }

        private IEnumerable<ConstructorInfo> GetConstructorsForVersionedService(Type implementationType)
        {
            IEnumerable<ConstructorInfo> implementationConstructors = implementationType.GetConstructors();

            var decoratedConstructors = implementationConstructors
                .Where(c => c.GetCustomAttribute<VersionedServiceConstructorAttribute>() != null)
                .ToList();

            //If there are any decorated constructors, then we'll only use them
            if (decoratedConstructors.Any())
            {
                return decoratedConstructors
                    .OrderBy(c => c.GetCustomAttribute<VersionedServiceConstructorAttribute>().Priority);
            }

            var ctorCount = implementationConstructors.Count();
            if (ctorCount == 0)
            {
                throw new InvalidOperationException($"Unable to determine which constructor to use to instatiate type {implementationType.FullName}. No constructor found.");
            }
            else if (ctorCount > 1)
            {
                throw new InvalidOperationException($"Unable to determine which constructor to use to instatiate type {implementationType.FullName}. More than one undecorated constructor provided.");
            }

            return implementationConstructors.Take(1);
        }

        private IEnumerable<ConstructorInfo> GetConstructorsForService(Type implementationType)
        {
            IEnumerable<ConstructorInfo> implementationConstructors = implementationType.GetConstructors();

            var ctorCount = implementationConstructors.Count();
            if (ctorCount == 0)
            {
                throw new InvalidOperationException($"Unable to determine which constructor to use to instatiate type {implementationType.FullName}. No constructor found.");
            }
            else if (ctorCount > 1)
            {
                throw new InvalidOperationException($"Unable to determine which constructor to use to instatiate type {implementationType.FullName}. More than one undecorated constructor provided.");
            }

            return implementationConstructors.Take(1);
        }
    }
}
