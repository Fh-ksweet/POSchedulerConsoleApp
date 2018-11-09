using DISchedulerConsoleApp.Model.Enums;
using System;

namespace DISchedulerConsoleApp.Model
{
    public class NextRun : EntityBase
    {
        public DateTime RunComplete { get; set; }
        public NextRunStatusType Status { get; set; }
    }
}
