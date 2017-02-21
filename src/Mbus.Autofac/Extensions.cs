using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus.Autofac
{
    using System.ComponentModel.Design;

    using global::Autofac;

    public static class Extensions
    {
        public static BusSetup UseAutofac(this BusSetup setup, ILifetimeScope componentContext)
        {
            setup.AddService<IComponentContext>(componentContext);
            setup.AddService<ILifetimeScope>(componentContext);
            setup.AddService<IServiceContainer>(c =>new ScopeServiceContainer(componentContext, c));
            return setup;
        }
    }
}
