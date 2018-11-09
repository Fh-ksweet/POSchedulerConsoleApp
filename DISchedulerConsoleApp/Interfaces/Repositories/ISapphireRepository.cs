using DISchedulerConsoleApp.Model;
using System;
using System.Collections.Generic;

namespace DISchedulerConsoleApp.Interfaces.Repositories
{
    public interface ISapphireRepository
    {
        List<QueueItem> GetFirstBatchQueueItems(DateTime lastCompletedDate);
        List<QueueItem> GetSecondBatchQueueItems(DateTime lastCompletedDate);
    }
}
