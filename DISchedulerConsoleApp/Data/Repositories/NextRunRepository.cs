using DISchedulerConsoleApp.Data.Contexts;
using DISchedulerConsoleApp.Interfaces.Repositories;
using DISchedulerConsoleApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace DISchedulerConsoleApp.Data.Repositories
{
    public class NextRunRepository : INextRunRepository
    {
        private readonly ProcessorDbContext _db;

        public NextRunRepository(ProcessorDbContext db)
        {
            _db = db;
        }

        public NextRun Create(NextRun nextRunToCreate)
        {
            _db.NextRuns.Add(nextRunToCreate);
            _db.SaveChanges();

            return nextRunToCreate;
        }

        public NextRun GetById(int id)
        {
            return _db.NextRuns.SingleOrDefault(n => n.Id == id);
        }

        public NextRun GetMostRecent()
        {
            return _db.NextRuns
                .OrderByDescending(n => n.Id)
                .FirstOrDefault();
        }

        public IList<NextRun> ListNextRuns()
        {
            return _db.NextRuns.ToList();
        }
    }
}
