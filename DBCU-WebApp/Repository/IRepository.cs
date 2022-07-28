using DBCU_WebApp.Models;
using DBCU_WebApp.Models.GeoClearance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {

        ValueTask<int> GetMissonEOD();
        //ValueTask<int> GetMissonNTS();
        //ValueTask<int> GetMissonTS();
        //ValueTask<int> GetMissonCLC();
 

        Task<List<GeoClearanceData>> GetGeoClearance();
        Task<List<GeoClearanceData>> GetGeoCHA();
    }
}
