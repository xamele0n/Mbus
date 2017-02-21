namespace Mbus.Publisher
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPublishPipelineMember
    {
        Task Publish(IEnumerable<Func<Task>> actions);
    }
}