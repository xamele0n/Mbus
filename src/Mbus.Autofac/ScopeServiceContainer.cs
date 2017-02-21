using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus.Autofac
{
    using System.ComponentModel.Design;

    using global::Autofac;

    public class ScopeServiceContainer : IServiceContainer
    {
        private readonly ILifetimeScope scope;

        private readonly IServiceContainer rootContainer;

        public ScopeServiceContainer(ILifetimeScope scope, IServiceContainer rootContainer)
        {
            this.scope = scope;
            this.rootContainer = rootContainer;
        }
        /// <summary>Gets the service object of the specified type.</summary>
        /// <returns>A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        public object GetService(Type serviceType)
        {
            object service;
            if (!this.scope.TryResolve(serviceType, out service))
            {
                service = this.rootContainer?.GetService(serviceType);
            }
            return service;
        }

        /// <summary>Adds the specified service to the service container.</summary>
        /// <param name="serviceType">The type of service to add. </param>
        /// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the <paramref name="serviceType" /> parameter. </param>
        public void AddService(Type serviceType, object serviceInstance)
        {
            throw new NotSupportedException();
        }

        /// <summary>Adds the specified service to the service container, and optionally promotes the service to any parent service containers.</summary>
        /// <param name="serviceType">The type of service to add. </param>
        /// <param name="serviceInstance">An instance of the service type to add. This object must implement or inherit from the type indicated by the <paramref name="serviceType" /> parameter. </param>
        /// <param name="promote">true to promote this request to any parent service containers; otherwise, false. </param>
        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            throw new NotSupportedException();
        }

        /// <summary>Adds the specified service to the service container.</summary>
        /// <param name="serviceType">The type of service to add. </param>
        /// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested. </param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            throw new NotSupportedException();
        }

        /// <summary>Adds the specified service to the service container, and optionally promotes the service to parent service containers.</summary>
        /// <param name="serviceType">The type of service to add. </param>
        /// <param name="callback">A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested. </param>
        /// <param name="promote">true to promote this request to any parent service containers; otherwise, false. </param>
        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            throw new NotSupportedException();
        }

        /// <summary>Removes the specified service type from the service container.</summary>
        /// <param name="serviceType">The type of service to remove. </param>
        public void RemoveService(Type serviceType)
        {
            throw new NotSupportedException();
        }

        /// <summary>Removes the specified service type from the service container, and optionally promotes the service to parent service containers.</summary>
        /// <param name="serviceType">The type of service to remove. </param>
        /// <param name="promote">true to promote this request to any parent service containers; otherwise, false. </param>
        public void RemoveService(Type serviceType, bool promote)
        {
            throw new NotSupportedException();
        }
    }
}
