using DISchedulerConsoleApp.Interfaces.Repositories;
using DISchedulerConsoleApp.Interfaces.Services;
using DISchedulerConsoleApp.Model;
using DISchedulerConsoleApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DISchedulerConsoleApp.Data.Repositories
{
    public class SapphireRepository : ISapphireRepository
    {
        private readonly IADOQueryService _adoQueryService;

        public SapphireRepository(IADOQueryService adoQueryService)
        {
            _adoQueryService = adoQueryService;
        }

        public List<QueueItem> GetFirstBatchQueueItems(DateTime lastCompletedDate)
        {
            var returnList = new List<QueueItem>();

            var firstQueueBatch = _adoQueryService.QueryADO(GetSapphireQueryString(lastCompletedDate));
            if (firstQueueBatch != null)
            {
                returnList.AddRange(firstQueueBatch.AsEnumerable().Select(MapToQueueItem).ToList());
            }

            return !returnList.Any() ? null : returnList;
        }

        public List<QueueItem> GetSecondBatchQueueItems(DateTime lastCompletedDate)
        {
            var returnList = new List<QueueItem>();

            var secondQueueBatch = _adoQueryService.QueryADO(GetSapphireSecondQueueBatch(lastCompletedDate));
            if (secondQueueBatch != null)
            {
                returnList.AddRange(secondQueueBatch.AsEnumerable().Select(MapToQueueItem).ToList());
            }

            return !returnList.Any() ? null : returnList;
        }

        private QueueItem MapToQueueItem(DataRow drow)
        {
            var queueItem = new QueueItem
            {
                JobRID = (drow["JobRID"].ToString() == "") ? "null" : drow["JobRID"].ToString(),
                SiteNumber = (drow["SiteNumber"].ToString() == "") ? "" : drow["SiteNumber"].ToString(),
                SapphirePONumber = (drow["SapphirePONumber"].ToString() == "") ? "" : drow["SapphirePONumber"].ToString(),
                Activity = (drow["activity"].ToString() == "") ? 1 : Convert.ToInt32(drow["activity"]),
                Name = (drow["Name"].ToString() == "") ? "" : drow["Name"].ToString(),
                NewJobNumber = (drow["NewJobNumber"].ToString() == "") ? "" : drow["NewJobNumber"].ToString(),
                Potype = (drow["po_type"].ToString() == "") ? "" : drow["po_type"].ToString(),
                SapphireObjID = (drow["SapphireObjID"].ToString() == "") ? "" : drow["SapphireObjID"].ToString(),
                SapphireObjRId = (drow["SapphireObjRID"].ToString() == "") ? 1 : Convert.ToInt32(drow["SapphireObjRID"]),
                Vendorid = (drow["Vendor_ID"].ToString() == "") ? "" : drow["Vendor_ID"].ToString(),
                PaymentAmount = (drow["payment_amount"].ToString() == "") ? (double?)null : Convert.ToSingle(drow["payment_amount"]),
                Subtotal = (drow["subtotal"].ToString() == "") ? (double?)null : Convert.ToSingle(drow["subtotal"]),
                Tax = (drow["tax"].ToString() == "") ? (double?)null : Convert.ToSingle(drow["tax"]),
                Total = (drow["total"].ToString() == "") ? 1 : Convert.ToSingle(drow["total"]) * -1,
                EgmAmount = (drow["EGMAmount"].ToString() == "") ? (double?)null : Convert.ToDouble(drow["EGMAmount"]),
                VpoYesNo = (!drow["vpo_yes_no"].Equals(0)) && Convert.ToBoolean(drow["vpo_yes_no"]),
                Community = (drow["Community"].ToString() == "") ? "" : ParseCommunityCode(drow["Community"].ToString()),
                Product = (drow["Product"].ToString() == "") ? "" : drow["Product"].ToString(),
                Building = (drow["Building"].ToString() == "") ? "" : drow["Building"].ToString(),
                Unit = (drow["Unit"].ToString() == "") ? "" : drow["Unit"].ToString(),
                TaxableAmount = (drow["taxable_amount"].ToString() == "") ? (double?)null : Convert.ToSingle(drow["taxable_amount"]),
                JobNo = (drow["job_no"].ToString() == "") ? "" : drow["job_no"].ToString(),
                TaxRate = (drow["TaxRate"].ToString() == "") ? (double?)null : Convert.ToSingle(drow["TaxRate"]),
                EMeasurementPO = (!drow["eMeasurementPO"].Equals(0)) && Convert.ToBoolean(drow["eMeasurementPO"]),
                Invoice = (drow["Invoice"].ToString() == "") ? "" : drow["Invoice"].ToString(),
                Status = QueueItemStatusType.New,
                LoadDateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                JobPOStatus = (drow["JobPOStatus"].ToString() == "") ? "" : drow["JobPOStatus"].ToString(),
                ESubmittalDate = SetDate(drow["eSubmittalDate"]),
                ApprovePaymentDate = SetDate(drow["ApprovePaymentDate"]),
                Cancelled_Date = SetDate(drow["Cancelled_Date"]),
                Release_Date = SetDate(drow["Release_Date"]),
                LastUpdated = SetDate(drow["LastUpdated"])
            };

            // Replace SetDates function by adding individual date fields to Queue object above, call new method "SetDate" to format all date fields
            //SetDates(drow, queueItem);

            return queueItem;
        }

        private string ParseCommunityCode(string Community)
        {
            // Community code is returned from Sapphire db as 8-character string (e.g. 'INR-DESN', 'ABN-MAST')
            //   This process should parse out and return the first 3 characters prior to the hyphen as community code
            if (!String.IsNullOrEmpty(Community) && Community.IndexOf("-") > -1)
            {
                return Community.Substring(0, 3);
            }

            return null;
        }

        private string GetJobNoFromJobID(string JobID)
        {
            if (!String.IsNullOrEmpty(JobID) && JobID.IndexOf("#") > -1)
            {
                return JobID.Substring(0, 12);
            }

            return null;
        }

        private DateTime? SetDate(object date)
        {
            var sapphireNullDateValue = new DateTime(1900, 1, 1);

            var newDate = (date.ToString() == "") ? (DateTime?)null : Convert.ToDateTime(date);

            return newDate.Equals(sapphireNullDateValue) ? null : newDate;
        }

        private void SetDates(DataRow drow, QueueItem queueItem)
        {
            var sapphireNullDateValue = new DateTime(1900, 1, 1);

            queueItem.ESubmittalDate = (drow["eSubmittalDate"].ToString() == "") ? (DateTime?)null : Convert.ToDateTime(drow["eSubmittalDate"]);
            if (queueItem.ESubmittalDate == sapphireNullDateValue)
            {
                queueItem.ESubmittalDate = null;
            }

            queueItem.ApprovePaymentDate = (drow["ApprovePaymentDate"].ToString() == "") ? (DateTime?)null : Convert.ToDateTime(drow["ApprovePaymentDate"]);
            if (queueItem.ApprovePaymentDate == sapphireNullDateValue)
            {
                queueItem.ApprovePaymentDate = null;
            }

            queueItem.CancelledDate = (drow["Cancelled_Date"].ToString() == "") ? (DateTime?)null : Convert.ToDateTime(drow["Cancelled_Date"]);
            if (queueItem.CancelledDate == sapphireNullDateValue)
            {
                queueItem.CancelledDate = null;
            }

            queueItem.ReleaseDate = (drow["Release_Date"].ToString() == "") ? (DateTime?)null : Convert.ToDateTime(drow["Release_Date"]);
            if (queueItem.ReleaseDate == sapphireNullDateValue)
            {
                queueItem.ReleaseDate = null;
            }
        }

        private string GetSapphireQueryString(DateTime lastRunDate)
        {
            return "SELECT " +
                                "Jobs.JobRID, " +
                                "left(Jobs.JobID, 12) as SiteNumber, " +
                                "Left(Jobs.JobID,5)+'/'+Substring(Jobs.JobID,6,3)+'/'+Substring(Jobs.JobID,9,4) as NewJobNumber, " +
                                "Acts.ActID as activity, " +
                                "Acts.Name AS Name, " +
                                "coalesce(Vnds.VndID, 'YNH00') as Vendor_ID, " +
                                "iif(coalesce(JobPOs.Status, '') in ('',NULL, 'Hold'),'H', 'Y') as po_type,  " +
                                "JobPOs.DateReleased as Release_Date, " +
                                "JobPOs.DateCancelled as Cancelled_Date, " +
                                "JobPOs.RefNumber as Invoice, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtPaid) as Payment_Amount, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtSubTotal) as Subtotal, " +
                                "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtTax) as Tax, " +
                                "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, coalesce(JobPOs.AmtTotal,JobCstActs.BudgetAmt)) as Total, " +
                                "iif( (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID) <= 1, coalesce(JobCstActs.BudgetAmt, JobPOs.AmtTotal), " +
                                    "iif(JobPOs.JobPORID = (select min(jpos4.JobPORID) from JobPOs jpos4 where JobPOs.JobRID = jpos4.JobRID and JobPOs.ActRID = jpos4.ActRID), coalesce(JobCstActs.BudgetAmt, JobPOs.AmtTotal), 0)) " +
                                    "as EGMAmount, " +
                                "0 as VPO_Yes_No, " +
                                "Comms.CommunityID as Community, " +
                                "Substring(Jobs.JobID,4,2) as Product, " +
                                "Substring(Jobs.JobID,6,3) as Building, " +
                                "Substring(Jobs.JobID,9,4) as Unit," +
                                "JobPOs.AmtTaxable as Taxable_Amount, " +
                                "Jobs.JobID as Job_No, " +
                                "JobPOs.DateApproved as ApprovePaymentDate, " +
                                "JobPOs.TaxPercentage as TaxRate, " +
                                "0 as eMeasurementPO," +
                                "j1.DateComplByVnd as eSubmittalDate, " +
                                "JobPOs.JobPOID as SapphirePONumber, " +
                                "iif(JobPOs.DateReleased is null,'JobCstActs', 'JobPOs') as SapphireObjID, " +
                                "iif(JobPOs.DateReleased is null,JobCstActs.JobCstActRID, JobPOs.JobPORID) as SapphireObjRID  " +
                                "JobPOStatus = JobPOs.Status, " +
                                "LastUpdated = COALESCE(JobPOs.LastUpdated, JobCstActs.LastUpdated) " +
                            "FROM " +
                            "    Jobs " +
                            "INNER JOIN Lots  " +
                            "    ON Jobs.LotRID = Lots.LotRID " +
                            "INNER JOIN Communities Comms " +
                            "    ON Lots.CommunityRID = Comms.CommunityRID " +
                            "INNER JOIN JobCstHdrs " +
                            "    ON Jobs.JobRID = JobCstHdrs.JobRID " +
                            "INNER JOIN JobCstActs " +
                            "    ON JobCstHdrs.JobCstHdrRID = JobCstActs.JobCstHdrRID " +
                            "INNER JOIN Acts " +
                            "    ON JobCstActs.ActRID = Acts.ActRID " +
                            "LEFT OUTER JOIN JobPOs " +
                            "    ON JobCstActs.JobRID = JobPOs.JobRID " +
                            "    AND JobCstActs.ActRID = JobPOs.ActRID " +
                            "LEFT OUTER JOIN Vnds " +
                            "    ON JobPOs.VndRID = Vnds.VndRID " +
                            "LEFT JOIN JobSchActs j1 " +
                            "   ON j1.JobActRID = JobPOs.JobActRID   " +
                            "   AND j1.VndRID = JobPOs.VndRID  " +
                            "LEFT JOIN Vnds v1 " +
                            "    ON j1.VndRID = v1.VndRID " +
                            "WHERE " +
                                "(JobCstHdrs.jobcsthdrrid in (select max(jobcsthdrrid) " +
                                 "from JobCstHdrs j3 " +
                                 "where j3.status in ('Current') " +
                                 "group by Name) " +
                                " AND coalesce(JobPOs.AmtTotal,JobCstActs.BudgetAmt) > 0.0 " +
                                   "AND " +
                                   "Jobs.DataProcStatus = 'Completed' AND " +
                                   "Comms.ShortID != 'TEST' AND " +
                                   "(COALESCE(JobPOs.LastUpdated, JobCstActs.LastUpdated)  > " + "'" + lastRunDate + "') " +
                                   "order by newjobnumber, activity; ";
        }

        private string GetSapphireSecondQueueBatch(DateTime lastRunDate)
        {
            return "SELECT " +
                            "Jobs.JobRID, " +
                            "left(Jobs.JobID, 12) as SiteNumber,  " +
                            "Left(Jobs.JobID,5)+'/'+Substring(Jobs.JobID,6,3)+'/'+Substring(Jobs.JobID,9,4) as NewJobNumber, " +
                            "Acts.ActID as activity, " +
                            "Acts.Name AS Name, " +
                            "coalesce(Vnds.VndID, 'YNH00') as Vendor_ID, " +
                            "iif(coalesce(JobPOs.Status, '') in ('',NULL, 'Hold'),'H', 'Y') as po_type,  " +
                            "JobPOs.DateReleased as Release_Date, " +
                            "JobPOs.DatePaid as Payment_Date, " +
                            "JobPOs.DateCancelled as Cancelled_Date, " +
                            "JobPOs.RefNumber as Invoice, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtPaid) as Payment_Amount, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtSubTotal) as Subtotal, " +
                            "iif(JobPOs.RAssnToJobPORID <> 0, 0, JobPOs.AmtTax) as Tax, " +
                            "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, JobPOs.AmtTotal) as Total, " +
                            "iif(JobPOs.DateCancelled > '1900-01-01' and (select count(*) from JobPOs jpos where JobPOs.JobRID = jpos.JobRID and JobPOs.ActRID = jpos.ActRID and jpos.DateCancelled = '1900-01-01') > 0 , 0, JobPOs.AmtTotal) as EGMAmount, " +

                            "JobPOs.CheckID as Check_No, " +
                            "0 as VPO_Yes_No, " +
                            "Left(Jobs.JobID,3) as Community, " +
                            "Substring(Jobs.JobID,4,2) as Product, " +
                            "Substring(Jobs.JobID,6,3) as Building, " +
                            "Substring(Jobs.JobID,9,4) as Unit, " +
                            "JobPOs.AmtTaxable as Taxable_Amount, " +
                            "Jobs.JobID as Job_No, " +
                            "JobPOs.DateApproved as ApprovePaymentDate, " +
                            "JobPOs.TaxPercentage as TaxRate, " +
                            "0 as eMeasurementPO, " +
                            "j1.DateComplByVnd as eSubmittalDate, " +
                            "JobPOs.JobPOID as SapphirePONumber, " +
                            "'JobPOs' as SapphireObjID, " +
                            "JobPOs.JobPORID as SapphireObjRID  " +
                        "FROM " +
                        "    Jobs " +
                        "INNER JOIN JobPOs " +
                        "    ON Jobs.JobRID = JobPOs.JobRID " +
                        "INNER JOIN Acts " +
                        "   ON JobPOs.ActRID = Acts.ActRID " +
                        " LEFT OUTER JOIN Vnds " +
                        "     ON JobPOs.VndRID = Vnds.VndRID " +
                        "INNER JOIN JobSchActs j1 " +
                        "   ON JobPOs.JobActRID = j1.JobActRID " +
                        "   AND j1.VndRID = JobPOs.VndRID " +
                        "WHERE " +
                        "    (JobPOs.Status IN ('Approved', 'Released', 'Hold', 'Cancelled', 'Completed', 'WorkInProgress')  OR JobPOs.Status IS NULL) AND " +
                        "   convert(varchar,jobpos.jobrid)+convert(varchar,jobpos.actrid) not in (select convert(varchar,jobcstacts.jobrid)+convert(varchar,jobcstacts.actrid) from jobcstacts where jobpos.jobrid = jobcstacts.jobrid) " +
                        "AND " +
                        "(JobPOs.LastUpdated >  " + "'" + lastRunDate + "'" + " or JobPOs.LastUpdated is null) " +
                        "order by newjobnumber, activity;";
        }
    }
}
