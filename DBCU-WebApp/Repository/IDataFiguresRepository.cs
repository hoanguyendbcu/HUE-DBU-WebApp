using DBCU_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Repository
{
    interface IDataFiguresRepository<T> where T : BaseEntity
    {
    }
}
