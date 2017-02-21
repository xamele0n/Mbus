using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    public delegate Task AsyncEventHandler<TEnvetArgs>(object sender, TEnvetArgs args);
}
