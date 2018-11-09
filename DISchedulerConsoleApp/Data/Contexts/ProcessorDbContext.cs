using DISchedulerConsoleApp.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DISchedulerConsoleApp.Data.Contexts
{
    public class ProcessorDbContext : DbContext
    {
        public ProcessorDbContext() : base("AWS_DIProcessor")
        {
            Database.SetInitializer<ProcessorDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<NextRun> NextRuns { get; set; }
        public DbSet<QueueItem> QueueItems { get; set; }
        public DbSet<ServiceStatus> ServiceStatuses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //EfMapNextRun(modelBuilder);
            EfMapQueueItem(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void EfMapNextRun(DbModelBuilder modelBuilder)
        {
            var nextRun = modelBuilder.Entity<NextRun>();
            nextRun.ToTable("NextRun");
        }

        private static void EfMapQueueItem(DbModelBuilder modelBuilder)
        {
            var item = modelBuilder.Entity<QueueItem>();
            item.ToTable("DIQueue");
        }
    }
}
