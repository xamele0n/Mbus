using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    public sealed class Disposable: IDisposable
    {
        private readonly Action disposeAction;

        private Disposable(Action disposeAction)
        {
            if(disposeAction == null)
                throw new ArgumentNullException(nameof(disposeAction));
            this.disposeAction = disposeAction;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.disposeAction();
        }

        public static IDisposable Create(Action disposeAction)
        {
            return new Disposable(disposeAction);
        }
    }
}
