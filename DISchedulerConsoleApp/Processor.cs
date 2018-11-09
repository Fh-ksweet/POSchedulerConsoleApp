using DISchedulerConsoleApp.Interfaces;
using DISchedulerConsoleApp.Interfaces.Repositories;
using DISchedulerConsoleApp.Interfaces.Services;
using DISchedulerConsoleApp.Model;
using System;
using System.Collections.Generic;

namespace DISchedulerConsoleApp
{
    public class Processor : IProcessor
    {
        private readonly INextRunRepository _nextRunRepository;
        private readonly ISapphireService _sapphireService;
        private readonly IQueueRepository _queueRepository;

        public Processor(INextRunRepository nextRunRepository, ISapphireService sapphireService, IQueueRepository queueRepository)
        {
            _nextRunRepository = nextRunRepository;
            _sapphireService = sapphireService;
            _queueRepository = queueRepository;
        }

        public void RunApp()
        {
            NextRun mostRecentNextRun = _nextRunRepository.GetMostRecent();
            if (mostRecentNextRun == null) { return; }

            var currentNextRun = new NextRun { RunComplete = DateTime.Now };

            List<QueueItem> nextBatchToProcess = _sapphireService.ListNextBatch(mostRecentNextRun.RunComplete);
            if (nextBatchToProcess == null) { return; }

            _queueRepository.Create(nextBatchToProcess);

            //currentNextRun.Status = NextRunStatusType.Processed;
            //_nextRunRepository.Create(currentNextRun);
        }
    }
}
