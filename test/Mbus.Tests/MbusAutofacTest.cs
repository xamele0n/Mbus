using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus.Tests
{
    using Autofac;

    using global::Autofac;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MbusAutofacTest
    {
        [TestMethod]
        public async Task IocTest()
        {
            var cb = new ContainerBuilder().Build();
            var bus = new BusSetup().Default().UseAutofac(cb).Build();
            await Task.Delay(0);
        }
    }
}
