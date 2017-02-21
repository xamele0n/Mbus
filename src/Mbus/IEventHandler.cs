using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    public interface IEventHandler<TEventArg>
    {
        Task Handle(object sender, TEventArg arg);
    }
}
