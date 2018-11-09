using DISchedulerConsoleApp.Model.Enums;
using System;

namespace DISchedulerConsoleApp.Model
{
    public class QueueItem : EntityBase
    {
        public int Activity { get; set; }
        public DateTime? ApprovePaymentDate { get; set; }
        public string Building { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string Community { get; set; }
        public double? EgmAmount { get; set; }
        public bool EMeasurementPO { get; set; }
        public DateTime? ESubmittalDate { get; set; }
        public string Invoice { get; set; }
        public string JobNo { get; set; }
        public string JobRID { get; set; }
        public string LoadDateTime { get; set; }
        public string Name { get; set; }
        public string NewJobNumber { get; set; }
        public double? PaymentAmount { get; set; }
        public string Potype { get; set; }
        public string Product { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string SapphireObjID { get; set; }
        public int SapphireObjRId { get; set; }
        public string SapphirePONumber { get; set; }
        public string SiteNumber { get; set; }
        public double? Subtotal { get; set; }
        public double? Tax { get; set; }
        public double? TaxableAmount { get; set; }
        public double? TaxRate { get; set; }
        public double? Total { get; set; }
        public string Unit { get; set; }
        public string Vendorid { get; set; }
        public bool VpoYesNo { get; set; }
        public QueueItemStatusType Status { get; set; }
        public string JobPOStatus { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? Cancelled_Date { get; set; }
        public DateTime? Release_Date { get; set; }
    }
}
