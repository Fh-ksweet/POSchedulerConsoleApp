using DISchedulerConsoleApp.Model.Enums;

namespace DISchedulerConsoleApp.Model
{
    public class ServiceStatus : EntityBase
    {
        public string ServiceName { get; set; }
        public ServiceStatusTypeValues Status { get; set; }
    }
}
