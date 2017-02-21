using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    public interface IPublisher
    {
        Task FireAsync<TEventArg>(object sender, TEventArg args);
    }
}
