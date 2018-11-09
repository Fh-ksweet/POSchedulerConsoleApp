using DISchedulerConsoleApp.Data.Contexts;
using DISchedulerConsoleApp.Interfaces.Repositories;
using DISchedulerConsoleApp.Model;
using System.Collections.Generic;

namespace DISchedulerConsoleApp.Data.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        public ProcessorDbContext _db;

        public QueueRepository(ProcessorDbContext db)
        {
            _db = db;
        }

        public void Create(QueueItem item)
        {
            _db.QueueItems.Add(item);
            _db.SaveChanges();
        }

        public void Create(IList<QueueItem> itemsToAdd)
        {
            _db.QueueItems.AddRange(itemsToAdd);
            _db.SaveChanges();
        }
    }
}
