using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mbus.Publisher;

namespace Mbus
{
    internal class EventHandlerCache : ReaderWriterCache<Type, Bus.EventRef>
    {
        private IPublishPipelineMember publisher;

        public EventHandlerCache(IPublishPipelineMember publisher):base()
        {
            this.publisher = publisher;
        }

        public Bus.EventRef Get(Type type)
        {
            return base.FetchOrCreateItem(type, () => new Bus.EventRef(this.publisher));
        }
    }
}
