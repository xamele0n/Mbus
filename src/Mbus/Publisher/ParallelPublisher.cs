using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus.Publisher
{
    public class ParallelPublisher : IPublishPipelineMember
    {
        public Task Publish(IEnumerable<Func<Task>> actions)
        {
           return Task.WhenAll(actions.Select(a => a?.Invoke()));
        }
    }
}
