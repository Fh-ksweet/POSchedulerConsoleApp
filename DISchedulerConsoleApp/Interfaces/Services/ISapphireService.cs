using DISchedulerConsoleApp.Model;
using System;
using System.Collections.Generic;

namespace DISchedulerConsoleApp.Interfaces.Services
{
    public interface ISapphireService
    {
        List<QueueItem> ListNextBatch(DateTime lastCompletedDate);
    }
}
