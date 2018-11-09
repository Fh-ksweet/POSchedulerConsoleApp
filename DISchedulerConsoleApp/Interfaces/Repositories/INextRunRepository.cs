using DISchedulerConsoleApp.Model;
using System.Collections.Generic;

namespace DISchedulerConsoleApp.Interfaces.Repositories
{
    public interface INextRunRepository
    {
        NextRun Create(NextRun nextRunToCreate);
        NextRun GetById(int id);
        NextRun GetMostRecent();
        IList<NextRun> ListNextRuns();
    }
}
