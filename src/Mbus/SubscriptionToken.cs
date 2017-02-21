using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    class SubscriptionToken : IDisposable
    {
        private readonly ICollection<IDisposable> disposables = new List<IDisposable>();
        private volatile bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddForDispose(IDisposable disposable)
        {
            this.disposables.Add(disposable);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                //TODO:Dispose managed resources here
                var _disposables = this.disposables.ToArray();
                this.disposables.Clear();
                foreach (var disposable in _disposables)
                {
                    disposable.Dispose();
                }
            }
            this.disposed = true;
        }

        public AsyncEventHandler<object> Handler { get; set; }
    }
}
