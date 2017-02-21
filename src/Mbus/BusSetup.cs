using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    using System.ComponentModel.Design;

    using Publisher;

    public class BusSetup
    {
        private IServiceContainer serviceContainer = new ServiceContainer();

        public BusSetup()
        {
            this.AddService<IBus>(c => new Bus(c));
        }
        public IBus Build()
        {
            CheckPrerequisites();
            return (IBus)serviceContainer.GetService(typeof(IBus));
        }

        public BusSetup Default()
        {
            return this.UsePublisher(new SequentalPublisher());
        }

        public BusSetup UsePublisher(IPublishPipelineMember publisher)
        {
            this.AddService(publisher);
            return this;
        }

        public BusSetup AddService<T>(T serviceInstance)
        {
            if (typeof(T) == typeof(IServiceContainer))
            {
                this.serviceContainer = (IServiceContainer)serviceInstance;
            }
            else
            {
                this.serviceContainer.AddService(typeof(T), serviceInstance);
            }
            
            return this;
        }

        public BusSetup AddService<T>(Func<IServiceContainer, T> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (typeof(T) == typeof(IServiceContainer))
            {
                this.serviceContainer = (IServiceContainer)factory(this.serviceContainer);
            }
            else
            {
                this.serviceContainer.AddService(typeof(T), (c, t) => factory(c));
            }
          
            return this;
        }

        private void CheckPrerequisites()
        {
            this.Check<IPublishPipelineMember>();
        }

        private void Check<T>()
        {
            var value = this.serviceContainer.GetService(typeof(T));
            if (value == null)
                throw new ArgumentNullException($"Required service '{typeof(T).Name}' is not registred");
        }
    }
}
