using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    using Publisher;

    public static class SetupExtension
    {
        public static BusSetup SequentalFire(this BusSetup setup)
        {
            return setup.UsePublisher(new SequentalPublisher());
        }

        public static BusSetup ParallelFire(this BusSetup setup)
        {
            return setup.UsePublisher(new ParallelPublisher());
        }
    }
}
