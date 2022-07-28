using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Npgsql;
using DBCU_WebApp.Models.Imsma;
using System.Threading.Tasks;
using DBCU_WebApp.Models.GeoClearance;
using DBCU_WebApp.Models.DataFigures;
using DBCU_WebApp.Models.GeoCHA;
using DBCU_WebApp.Models.BomBing;

namespace DBCU_WebApp.Repository
{
    public class MissonEODRepository : IRepository<WebDataHome>
    {
        private string connectionString;
        private string connectionString1;
        private string connectionStringDBCU;
        public MissonEODRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("ConnectionStrings:imsma");
            connectionString1 = configuration.GetValue<string>("ConnectionStrings:staging");
            connectionStringDBCU = configuration.GetValue<string>("ConnectionStrings:DBCU_Web");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        internal IDbConnection Connection1
        {
            get
            {
                return new NpgsqlConnection(connectionString1);
            }
        }
        internal IDbConnection connectionDBCU
        {
            get
            {
                return new NpgsqlConnection(connectionStringDBCU);
            }
        }
        public async ValueTask<int> GetMissonEOD()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = "select COUNT(*) from hazreduc a  ";
                strQuery = strQuery + " where (a.hazreduc_localid like '%EOD%' OR a.hazreduc_localid like '%Activity%') ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;

            }
        }
        //public async ValueTask<int> GetMissonNTS()
        //{
        //    using (IDbConnection dbConnection = Connection1)
        //    {
        //        dbConnection.Open();
        //        string strQuery = "select COUNT(*) from hazreduc a, imsmaenum b ";
        //        strQuery = strQuery + " where a.hazreductypeenum_guid = b.imsmaenum_guid";
        //        strQuery = strQuery + " and b.enumvalue = 'NTS'";
        //        var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
        //        dbConnection.Close();

        //        return returnValue;

        //    }
        //}
        //public async ValueTask<int> GetMissonTS()
        //{
        //    using (IDbConnection dbConnection = Connection1)
        //    {
        //        dbConnection.Open();
        //        string strQuery = "select COUNT(*) from hazreduc a, imsmaenum b ";
        //        strQuery = strQuery + " where a.hazreductypeenum_guid = b.imsmaenum_guid";
        //        strQuery = strQuery + " and b.enumvalue = 'Survey2'";
        //        var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
        //        dbConnection.Close();

        //        return returnValue;
        //    }
        //}
        //public async ValueTask<int> GetMissonCLC()
        //{
        //    using (IDbConnection dbConnection = Connection1)
        //    {
        //        dbConnection.Open();
        //        string strQuery = "select COUNT(*) from hazreduc a, imsmaenum b ";
        //        strQuery = strQuery + " where a.hazreductypeenum_guid = b.imsmaenum_guid";
        //        strQuery = strQuery + " and b.enumvalue in ('PostClearance','Post Technical Survey') ";
        //        var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
        //        dbConnection.Close();

        //        return returnValue;
        //    }
        //}

        public async ValueTask<int> GetAreaCLC()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = "select CAST( Sum(areasize) as int)  from hazreduc a where hazreduc_localid like '%CLC%' ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;              
            }
        }

        public async ValueTask<int> GetAreaCHA()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = "select CAST( Sum(areasize) as int)  from hazard ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }
        public async ValueTask<int> GetNoERW()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = "SELECT  SUM(qty) qty   ";
                strQuery = strQuery + " FROM hazreduc_point, hazreduc, hazreducdeviceinfo ";
                strQuery = strQuery + " WHERE hazreduc_point.point_type::text = 'Evidence Point'::text ";
                strQuery = strQuery + " AND hazreduc.hazreduc_guid::text = hazreduc_point.hazreduc_guid::text ";
                strQuery = strQuery + " AND hazreduc_point.hazreduc_guid::text = hazreducdeviceinfo.hazreduc_guid::text ";
                strQuery = strQuery + " AND hazreduc_point.point_local_id::text = hazreducdeviceinfo.resource::text ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetNoMRE()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = "SELECT CAST(SUM(coalesce(a.qty,0)+ coalesce(b.totalaudience,0) + coalesce(b.malepercentage,0) + coalesce(b.femalepercentage,0)) AS INT) qty   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<GeoClearanceData>> GetGeoClearance()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select lr_id,lr_id lr_name, status,reporting_team,reporting_org_name,to_char(a.startdate,'dd/MM/yyyy') startdate ";
                strQuery = strQuery + "  , to_char(a.enddate, 'dd/MM/yyyy') enddate,cast(round(a.areasize) as varchar) as areasize,village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "  from    dbcu_lr a ";

               // List<GeoClearanceData> returnValue = (await dbConnection.QueryAsync(strQuery)).ToList();
                List<GeoClearanceData> returnValue = new List<GeoClearanceData>(await dbConnection.QueryAsync<GeoClearanceData>(strQuery));
                dbConnection.Close();

                return  returnValue;
            }
        }

        public async Task<List<GeoClearanceData>> GetGeoCHA()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select cha_id lr_id, org_internal_id lr_name, status,reporting_org_name,to_char(a.cha_identification_date,'dd/MM/yyyy') startdate,";
                strQuery = strQuery + " cast(round(a.areasize) as varchar) as areasize,";
                strQuery = strQuery + " village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + " from    dbcu_cha a  ";

                // List<GeoClearanceData> returnValue = (await dbConnection.QueryAsync(strQuery)).ToList();
                List<GeoClearanceData> returnValue = new List<GeoClearanceData>(await dbConnection.QueryAsync<GeoClearanceData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<GeoCHAData>> GetGeoCHAOpen()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select cha_id lr_id, org_internal_id lr_name,";
                strQuery = strQuery + "    (CASE WHEN task.status_enum in ('Suspended', 'Ongoing', 'Completed') THEN task.status_enum ELSE status END) status, NULLIF(reporting_org_name, '')reporting_org_name,to_char(a.cha_identification_date, 'dd/MM/yyyy') startdate,";
                strQuery = strQuery + "   COALESCE(Reporting_Org_localid, '') Reporting_Org_localid,COALESCE(Survey_method, '') Survey_method,COALESCE(Land_Use, '') Land_Use, COALESCE(Comment_Land_Use, '') Comment_Land_Use,COALESCE(Beneficiaries, '') Beneficiaries, COALESCE(Clearance_Priority, '') Clearance_Priority,COALESCE(Type_of_Area, '') Type_of_Area,";
                strQuery = strQuery + "   COALESCE(Vehicle_Type, '') Vehicle_Type,COALESCE(Vegetation_removed_by, '') Vegetation_removed,COALESCE(Soil_type, '') Soiltype,  COALESCE(Vegetation_Type, '') Vegetation_Type,COALESCE(Vegetation_density, '') Vegetation_density,COALESCE(slopee, '') slopee,COALESCE(Soil_Condition, '') Soil_Condition, COALESCE(Additional_Information, '') Additional_Information,";
                strQuery = strQuery + "    cast(round(a.areasize) as varchar) as areasize, ";
                strQuery = strQuery + "    village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "    from    dbcu_cha a   left join(";
                strQuery = strQuery + "    select hazard_guid, status_enum, max(data_entry_date)";
                strQuery = strQuery + "    from  public.task_has_objective, public.task";
                strQuery = strQuery + "    where public.task.guid = public.task_has_objective.task_guid";
                strQuery = strQuery + "    group by hazard_guid, status_enum   ) task on task.hazard_guid = a.hazard_guid WHERE (CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END) ='Open'";

                List<GeoCHAData> returnValue = new List<GeoCHAData>(await dbConnection.QueryAsync<GeoCHAData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<GeoCHAData>> GetGeoCHASuspended()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select cha_id lr_id, org_internal_id lr_name,";
                strQuery = strQuery + "    (CASE WHEN task.status_enum in ('Suspended', 'Ongoing', 'Completed') THEN task.status_enum ELSE status END) status, NULLIF(reporting_org_name, '')reporting_org_name,to_char(a.cha_identification_date, 'dd/MM/yyyy') startdate,";
                strQuery = strQuery + "   COALESCE(Reporting_Org_localid, '') Reporting_Org_localid,COALESCE(Survey_method, '') Survey_method,COALESCE(Land_Use, '') Land_Use, COALESCE(Comment_Land_Use, '') Comment_Land_Use,COALESCE(Beneficiaries, '') Beneficiaries, COALESCE(Clearance_Priority, '') Clearance_Priority,COALESCE(Type_of_Area, '') Type_of_Area,";
                strQuery = strQuery + "   COALESCE(Vehicle_Type, '') Vehicle_Type,COALESCE(Vegetation_removed_by, '') Vegetation_removed,COALESCE(Soil_type, '') Soiltype,  COALESCE(Vegetation_Type, '') Vegetation_Type,COALESCE(Vegetation_density, '') Vegetation_density,COALESCE(slopee, '') slopee,COALESCE(Soil_Condition, '') Soil_Condition, COALESCE(Additional_Information, '') Additional_Information,";
                strQuery = strQuery + "    cast(round(a.areasize) as varchar) as areasize, ";
                strQuery = strQuery + "    village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "    from    dbcu_cha a   left join(";
                strQuery = strQuery + "    select hazard_guid, status_enum, max(data_entry_date)";
                strQuery = strQuery + "    from  public.task_has_objective, public.task";
                strQuery = strQuery + "    where public.task.guid = public.task_has_objective.task_guid";
                strQuery = strQuery + "    group by hazard_guid, status_enum   ) task on task.hazard_guid = a.hazard_guid WHERE (CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END) ='Suspended'";

                List<GeoCHAData> returnValue = new List<GeoCHAData>(await dbConnection.QueryAsync<GeoCHAData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<GeoCHAData>> GetGeoCHACompleted()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select cha_id lr_id, org_internal_id lr_name,";
                strQuery = strQuery + "    (CASE WHEN task.status_enum in ('Suspended', 'Ongoing', 'Completed') THEN task.status_enum ELSE status END) status, NULLIF(reporting_org_name, '')reporting_org_name,to_char(a.cha_identification_date, 'dd/MM/yyyy') startdate,";
                strQuery = strQuery + "   COALESCE(Reporting_Org_localid, '') Reporting_Org_localid,COALESCE(Survey_method, '') Survey_method,COALESCE(Land_Use, '') Land_Use, COALESCE(Comment_Land_Use, '') Comment_Land_Use,COALESCE(Beneficiaries, '') Beneficiaries, COALESCE(Clearance_Priority, '') Clearance_Priority,COALESCE(Type_of_Area, '') Type_of_Area,";
                strQuery = strQuery + "   COALESCE(Vehicle_Type, '') Vehicle_Type,COALESCE(Vegetation_removed_by, '') Vegetation_removed,COALESCE(Soil_type, '') Soiltype,  COALESCE(Vegetation_Type, '') Vegetation_Type,COALESCE(Vegetation_density, '') Vegetation_density,COALESCE(slopee, '') slopee,COALESCE(Soil_Condition, '') Soil_Condition, COALESCE(Additional_Information, '') Additional_Information,";
                strQuery = strQuery + "    cast(round(a.areasize) as varchar) as areasize, ";
                strQuery = strQuery + "    village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "    from    dbcu_cha a   left join(";
                strQuery = strQuery + "    select hazard_guid, status_enum, max(data_entry_date)";
                strQuery = strQuery + "    from  public.task_has_objective, public.task";
                strQuery = strQuery + "    where public.task.guid = public.task_has_objective.task_guid";
                strQuery = strQuery + "    group by hazard_guid, status_enum   ) task on task.hazard_guid = a.hazard_guid WHERE (CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END)  IN ('Completed','Closed')";

                List<GeoCHAData> returnValue = new List<GeoCHAData>(await dbConnection.QueryAsync<GeoCHAData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<GeoCHAData>> GetGeoCHAOngoing()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select cha_id lr_id, org_internal_id lr_name,";
                strQuery = strQuery + "    (CASE WHEN task.status_enum in ('Suspended', 'Ongoing', 'Completed') THEN task.status_enum ELSE status END) status, NULLIF(reporting_org_name, '')reporting_org_name,to_char(a.cha_identification_date, 'dd/MM/yyyy') startdate,";
                strQuery = strQuery + "   COALESCE(Reporting_Org_localid, '') Reporting_Org_localid,COALESCE(Survey_method, '') Survey_method,COALESCE(Land_Use, '') Land_Use, COALESCE(Comment_Land_Use, '') Comment_Land_Use,COALESCE(Beneficiaries, '') Beneficiaries, COALESCE(Clearance_Priority, '') Clearance_Priority,COALESCE(Type_of_Area, '') Type_of_Area,";
                strQuery = strQuery + "   COALESCE(Vehicle_Type, '') Vehicle_Type,COALESCE(Vegetation_removed_by, '') Vegetation_removed,COALESCE(Soil_type, '') Soiltype,  COALESCE(Vegetation_Type, '') Vegetation_Type,COALESCE(Vegetation_density, '') Vegetation_density,COALESCE(slopee, '') slopee,COALESCE(Soil_Condition, '') Soil_Condition, COALESCE(Additional_Information, '') Additional_Information,";
                strQuery = strQuery + "    cast(round(a.areasize) as varchar) as areasize, ";
                strQuery = strQuery + "    village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "    from    dbcu_cha a   left join(";
                strQuery = strQuery + "    select hazard_guid, status_enum, max(data_entry_date)";
                strQuery = strQuery + "    from  public.task_has_objective, public.task";
                strQuery = strQuery + "    where public.task.guid = public.task_has_objective.task_guid";
                strQuery = strQuery + "    group by hazard_guid, status_enum   ) task on task.hazard_guid = a.hazard_guid WHERE (CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END) ='Ongoing'";

                List<GeoCHAData> returnValue = new List<GeoCHAData>(await dbConnection.QueryAsync<GeoCHAData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        /// <summary>
        /// ///////////////////
        /// </summary>
        /// <returns></returns>
        ///          
 
        public async Task<List<GeoClearanceData>> GetGeoCLCCM()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select lr_id,lr_id lr_name, status,reporting_team,reporting_org_name,to_char(a.startdate,'dd/MM/yyyy') startdate ";
                strQuery = strQuery + "  , to_char(a.enddate, 'dd/MM/yyyy') enddate,cast(round(a.areasize) as varchar) as areasize,village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "  from    dbcu_lr a WHERE reporting_org_localid <> 'PMCQB'";

                // List<GeoClearanceData> returnValue = (await dbConnection.QueryAsync(strQuery)).ToList();
                List<GeoClearanceData> returnValue = new List<GeoClearanceData>(await dbConnection.QueryAsync<GeoClearanceData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<GeoClearanceData>> GetGeoCLCTM()
        {
            using (IDbConnection dbConnection = Connection1)
            {
                dbConnection.Open();
                string strQuery = " select lr_id,lr_id lr_name, status,reporting_team,reporting_org_name,to_char(a.startdate,'dd/MM/yyyy') startdate ";
                strQuery = strQuery + "  , to_char(a.enddate, 'dd/MM/yyyy') enddate,cast(round(a.areasize) as varchar) as areasize,village_name,commune_name,a.district_name,st_asgeojson(shape) polygon";
                strQuery = strQuery + "  from    dbcu_lr a WHERE reporting_org_localid = 'PMCQB' ";

                // List<GeoClearanceData> returnValue = (await dbConnection.QueryAsync(strQuery)).ToList();
                List<GeoClearanceData> returnValue = new List<GeoClearanceData>(await dbConnection.QueryAsync<GeoClearanceData>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<IEnumerable<QBBomBing3KMPoint>> GetDataBomBingQB()
        {
            using (IDbConnection dbConnection = connectionDBCU)
            {
                dbConnection.Open();
                var strQuery = @"select * from public.BomBingQB ";
                var returnValue = await dbConnection.QueryAsync<QBBomBing3KMPoint>(strQuery);
                dbConnection.Close();

                return returnValue;
            } 
        }

    }
}
