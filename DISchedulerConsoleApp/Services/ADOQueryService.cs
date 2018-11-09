using DISchedulerConsoleApp.Interfaces;
using DISchedulerConsoleApp.Interfaces.Services;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DISchedulerConsoleApp.Services
{
    public class ADOQueryService : IADOQueryService
    {
        private readonly ILog _logger;

        public ADOQueryService(ILog logger)
        {
            _logger = logger;
        }

        public DataTable QueryADO(string queryString)
        {
            var connString = ConfigurationManager.ConnectionStrings["SapphireDbContext"].ConnectionString;
            var oConnection = new SqlConnection(connString);
            oConnection.Open();
            var oCommand = new SqlCommand(queryString, oConnection);
            var oAdapter = new SqlDataAdapter(oCommand);
            var dt = new DataTable();

            try
            {
                oAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                _logger.Error("Database Failure ADO.NET");
            }
            finally
            {
                oConnection.Close();
            }

            return dt;
        }
    }
}
