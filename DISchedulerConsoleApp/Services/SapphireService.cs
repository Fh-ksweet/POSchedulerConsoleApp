using DISchedulerConsoleApp.Interfaces.Repositories;
using DISchedulerConsoleApp.Interfaces.Services;
using DISchedulerConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DISchedulerConsoleApp.Services
{
    public class SapphireService : ISapphireService
    {
        private readonly ISapphireRepository _sapphireRepository;

        public SapphireService(ISapphireRepository sapphireRepository)
        {
            _sapphireRepository = sapphireRepository;
        }

        public List<QueueItem> ListNextBatch(DateTime lastCompletedDate)
        {
            var returnList = new List<QueueItem>();

            var firstBatch = _sapphireRepository.GetFirstBatchQueueItems(lastCompletedDate);
            if (firstBatch != null)
            {
                returnList.AddRange(firstBatch);
            }

            var secondBatch = _sapphireRepository.GetSecondBatchQueueItems(lastCompletedDate);
            if (secondBatch != null)
            {
                returnList.AddRange(secondBatch);
            }

            return !returnList.Any() ? null : returnList;
        }
    }
}
