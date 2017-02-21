using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mbus.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;

    [TestClass]
    public class Mbus
    {
        private Random random = new Random((int)(DateTime.UtcNow.Ticks & int.MaxValue));
       

        [TestMethod]
        public void TestMethod1()
        {
            var setup = new BusSetup().Default();
            Mock<IEventHandler<string>> mock = new Mock<IEventHandler<string>>();
            mock.Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<string>())).Returns<object, string>(Write);
            using (var bus = setup.Build())
            {
                bus.Subscribe<string>(mock.Object.Handle);

                bus.FireAsync(this, "hello").Wait();
                bus.FireAsync(this, "hello").Wait();
                bus.FireAsync(this, "world").Wait();
                mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(3));
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var setup = new BusSetup().Default();
            Mock<IEventHandler<string>> mock = new Mock<IEventHandler<string>>();
            mock.Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<string>())).Returns<object, string>(Write);
            using (var bus = setup.Build())
            {
                bus.Subscribe<string>(mock.Object.Handle);
                bus.Subscribe<string>(mock.Object.Handle);

                bus.FireAsync(this, "hello").Wait();
                bus.FireAsync(this, "hello").Wait();
                bus.FireAsync(this, "world").Wait();
                mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(6));
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            var setup = new BusSetup().Default();
            Mock<IEventHandler<string>> mock = new Mock<IEventHandler<string>>();
            mock.Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<string>())).Returns<object, string>(Write);
            using (var bus = setup.Build())
            {
                using (bus.Subscribe<string>(mock.Object.Handle))
                {
                    bus.FireAsync(this, "hello").Wait();
                    mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(1));
                }
                using (bus.Subscribe<string>(mock.Object.Handle))
                {
                    bus.FireAsync(this, "world").Wait();
                    mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(2));
                }
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            var setup = new BusSetup().Default();
            Mock<IEventHandler<string>> mock = new Mock<IEventHandler<string>>();
            mock.Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<string>())).Returns<object, string>(Write);
            using (var bus =setup.Build())
            {
                using (bus.Subscribe<string>(mock.Object.Handle))
                {
                    using (bus.Subscribe<string>(mock.Object.Handle))
                    {
                        bus.FireAsync(this, "hello").Wait();
                        mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(2));
                    }
                    bus.FireAsync(this, "hello").Wait();
                    mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(3));

                }
                bus.FireAsync(this, "hello").Wait();
                mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(3));
                bus.FireAsync(this, "hello").Wait();
                mock.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<string>()), Times.Exactly(3));

            }
        }

        [TestMethod]
        public async Task PerformanceTest()
        {
            var setup = new BusSetup().Default();
            using (var bus = setup.Build())
            {
                foreach (var i in Enumerable.Range(1, 1000000))
                {
                    using (bus.Subscribe<EArg>(
                        (o, s) =>
                            {
                                return Task.FromResult(0);
                            }))
                    using (bus.Subscribe<string>((o, s) => Task.FromResult(0)))
                    using (bus.Subscribe<int>((o, s) => Task.FromResult(0)))
                    {
                        await bus.FireAsync(this, new EArg {Text = "test"});
                    }
                }
            }
        }

        private async Task Write(object sender, string message)
        {
            await Task.Delay(this.random.Next(1000));
            Trace.WriteLine(message);
        }

        public class EArg
        {
            public string Text { get; set; }
        }

        
    }
}
