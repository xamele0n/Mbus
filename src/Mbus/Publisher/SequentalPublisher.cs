using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbus.Publisher
{
    public class SequentalPublisher : IPublishPipelineMember
    {
        public async Task Publish(IEnumerable<Func<Task>> actions)
        {
            foreach (var action in actions)
            {
                await action.Invoke();
            }
        }
    }
}
