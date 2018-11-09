using System.Data;

namespace DISchedulerConsoleApp.Interfaces.Services
{
    public interface IADOQueryService
    {
        DataTable QueryADO(string queryString);
    }
}
