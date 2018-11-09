using DISchedulerConsoleApp.Model;
using System.Collections.Generic;

namespace DISchedulerConsoleApp.Interfaces.Repositories
{
    public interface IQueueRepository
    {
        void Create(QueueItem item);
        void Create(IList<QueueItem> itemsToAdd);
    }
}
