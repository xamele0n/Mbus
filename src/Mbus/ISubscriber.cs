using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    public interface ISubscriber
    {
        IDisposable Subscribe<TEventArgs>(AsyncEventHandler<TEventArgs> handler);
    }
}
