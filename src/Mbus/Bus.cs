using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus
{
    using System.Collections.Concurrent;
    using System.ComponentModel.Design;

    using Publisher;

    public class Bus : IBus
    {
        private volatile bool disposed;

        private readonly ICollection<IDisposable> disposables = new List<IDisposable>();

        private readonly EventHandlerCache handlers;

        public Bus(IServiceContainer serviceContainer)
        {
            var publisher = (IPublishPipelineMember)serviceContainer.GetService(typeof(IPublishPipelineMember));
            if (publisher == null) throw new ArgumentNullException(nameof(publisher));
            this.handlers = new EventHandlerCache(publisher);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (this.disposed) return;
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                //TODO:Dispose managed resources here
                IDisposable[] _disposables;
                lock (disposables)
                {
                    _disposables = this.disposables.ToArray();
                    this.disposables.Clear();
                }
                if (this.disposed) return;
                foreach (var disposable in _disposables)
                {
                    disposable?.Dispose();
                }
            }
            this.disposed = true;
        }

        private void CheckDisposed()
        {
            if (!this.disposed) return;
            throw new ObjectDisposedException("bus");
        }

        public async Task FireAsync<TEventArg>(object sender, TEventArg args)
        {
            this.CheckDisposed();
            EventRef _handler = this.handlers.Get(typeof(TEventArg));
            await _handler.Fire(sender, args);
        }

        public IDisposable Subscribe<TEventArg>(AsyncEventHandler<TEventArg> handler)
        {
            this.CheckDisposed();
            var _handler = this.handlers.Get(typeof(TEventArg));
            var token = new SubscriptionToken();
            token.Handler = (s, e) => handler(s, (TEventArg)e);
            _handler.Event += token.Handler;
            token.AddForDispose(Disposable.Create(
                () =>
                    {
                        _handler.Event -= token.Handler;
                        this.disposables.Remove(token);
                    }));
            this.disposables.Add(token);
            return token;
        }

        internal class EventRef
        {
            private readonly IPublishPipelineMember publisher;

            public EventRef(IPublishPipelineMember publisher)
            {
                this.publisher = publisher;
            }

            public event AsyncEventHandler<object> Event;

            public async Task Fire<TEventArg>(object sender, TEventArg args)
            {
                var _event = this.Event;
                if (_event == null) return;
                var invokeList = _event.GetInvocationList().Cast<AsyncEventHandler<object>>().Select(h => new Func<Task>(async () => await h.Invoke(sender, args)));
                await this.publisher.Publish(invokeList);
            }
        }
    }
}
