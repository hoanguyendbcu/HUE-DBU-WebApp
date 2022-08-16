using Dapper;
using DBCU_WebApp.Models.DataFigures;
using DBCU_WebApp.Models.GeoCHA;
using DBCU_WebApp.Models.Imsma;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Repository
{
    public class DataFiguresRepository : IDataFiguresRepository<WebDataHome>
    {
        private string connectionString;
        private string connectionStringStaging;
        public DataFiguresRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("ConnectionStrings:imsma");
            connectionStringStaging = configuration.GetValue<string>("ConnectionStrings:staging");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        internal IDbConnection ConnectionStaging
        {
            get
            {
                return new NpgsqlConnection(connectionStringStaging);
            }
        }

        public async ValueTask<int> GetAreaCLC(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "select coalesce(CAST( Sum(areasize) as int),0)  from hazreduc a where hazreduc_localid like '%CLC%' AND ('0'= '" + distict + "' OR gazetteer_level3_name = '" + distict + "' )";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetNoERW(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "SELECT  coalesce(SUM(qty),0) qty  FROM hazreduc_point, hazreduc, hazreducdeviceinfo";
                strQuery = strQuery + " WHERE hazreduc_point.point_type::text = 'Evidence Point'::text";
                strQuery = strQuery + " AND hazreduc.hazreduc_guid::text = hazreduc_point.hazreduc_guid::text";
                strQuery = strQuery + " AND hazreduc_point.hazreduc_guid::text = hazreducdeviceinfo.hazreduc_guid::text";
                strQuery = strQuery + " AND hazreduc_point.point_local_id::text = hazreducdeviceinfo.resource::text";
                strQuery = strQuery + " AND('0'= '" + distict + "' OR hazreduc.gazetteer_level3_name = '" + distict + "') ";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetNoMRE(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "SELECT CAST(coalesce(SUM(coalesce(a.qty,0)+ coalesce(b.totalaudience,0) + coalesce(b.malepercentage,0) + coalesce(b.femalepercentage,0)),0) AS INT) qty   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid WHERE ('0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "' )";
                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<UXOCategory>> UXOCategory(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "      select category, qty, color as backgroundColor from( ";
                strQuery = strQuery + "      SELECT  row_number() over() as id, category, qty FROM";
                strQuery = strQuery + "      (SELECT ordcategoryenum_enum as category, SUM(qty) qty  FROM public.hazreducdeviceinfo a, hazreduc b";
                strQuery = strQuery + "  where a.hazreduc_guid = b.hazreduc_guid AND resource like 'EV%' AND ( '0'= '" + distict + "' OR b.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "    GROUP BY ordcategoryenum_enum";
                strQuery = strQuery + "    ORDER BY SUM(qty) DESC LIMIT 5";
                strQuery = strQuery + "    )a)a,";
                strQuery = strQuery + "    (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "    SELECT unnest('{#815DF6, #67B7DC, #9c82f4,#FDD400, #FC0703} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id";
                List<UXOCategory> returnValue = new List<UXOCategory>(await dbConnection.QueryAsync<UXOCategory>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<UXOCategory>> UXOModel(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "      select category, qty, color as backgroundColor from( ";
                strQuery = strQuery + "      SELECT  row_number() over() as id, category, qty FROM";
                strQuery = strQuery + "      (SELECT model as category, SUM(qty) qty  FROM public.hazreducdeviceinfo a, hazreduc b";
                strQuery = strQuery + "  where a.hazreduc_guid = b.hazreduc_guid AND resource like 'EV%' AND ( '0'= '" + distict + "' OR b.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "    GROUP BY model";
                strQuery = strQuery + "    ORDER BY SUM(qty) DESC LIMIT 5";
                strQuery = strQuery + "    )a)a,";
                strQuery = strQuery + "    (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "    SELECT unnest('{#FC0703,#2EFC03, #815DF6, #67B7DC ,#FDD400} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id";
                List<UXOCategory> returnValue = new List<UXOCategory>(await dbConnection.QueryAsync<UXOCategory>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<MREGender>> GetMREGender(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select category, qty, color as backgroundColor from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT coalesce(SUM(coalesce(a.qty, 0)),0) qty, 'Male       ' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "  Union all";
                strQuery = strQuery + "  SELECT coalesce(SUM(coalesce(b.totalaudience,0)),0) qty,'Female     ' category FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#FC0703,#2EFC03} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                List<MREGender> returnValue = new List<MREGender>(await dbConnection.QueryAsync<MREGender>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<MREGender>> GetMREChildGender(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select category, qty, color as backgroundColor from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT coalesce(SUM(coalesce(b.malepercentage, 0)),0) qty, 'Male child' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "  Union all";
                strQuery = strQuery + "  SELECT coalesce(SUM(coalesce(b.femalepercentage,0)),0) qty,'Female child' category FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#030DFC,#E203FC} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                List<MREGender> returnValue = new List<MREGender>(await dbConnection.QueryAsync<MREGender>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetMREGenderMale (string org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select  qty from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT coalesce(SUM(coalesce(a.qty, 0)),0) qty, 'Male' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + org + "' OR a.org_localid = '" + org + "') "; 

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#FC0703,#2EFC03} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }
        public async ValueTask<int> GetMREGenderFemale(string org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select  qty from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM("; 
                strQuery = strQuery + "  SELECT coalesce(SUM(coalesce(b.totalaudience,0)),0) qty,'Female' category FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + org + "' OR a.org_localid = '" + org + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#FC0703,#2EFC03} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetMREChild(string org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select qty from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT (coalesce(SUM(coalesce(b.malepercentage, 0)),0) + coalesce(SUM(coalesce(b.femalepercentage, 0)),0) ) qty, 'Trẻ em trai' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + org + "' OR a.org_localid = '" + org + "') "; 

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#030DFC,#E203FC} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";

                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async ValueTask<int> GetMRETotal(string org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select qty from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT (coalesce(SUM(coalesce(a.qty, 0)),0) + coalesce(SUM(coalesce(b.totalaudience, 0)),0) + coalesce(SUM(coalesce(b.malepercentage, 0)),0) + coalesce(SUM(coalesce(b.femalepercentage, 0)),0) ) qty, 'Trẻ em trai' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + org + "' OR a.org_localid = '" + org + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#030DFC,#E203FC} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                var returnValue = await dbConnection.QueryFirstOrDefaultAsync<int>(strQuery);
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<MREGender>> GetMREByYear(string org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
 
                string strQuery  = "  SELECT   category, qty FROM(";
                strQuery = strQuery + "    SELECT (coalesce(SUM(coalesce(a.qty, 0)),0) + coalesce(SUM(coalesce(b.totalaudience, 0)),0) + coalesce(SUM(coalesce(b.malepercentage, 0)),0) + coalesce(SUM(coalesce(b.femalepercentage, 0)),0) ) qty,to_char(a.enddate, 'yyyy')  category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + org + "' OR a.org_localid = '" + org + "') ";
                strQuery = strQuery + "  GROUP BY to_char(a.enddate, 'yyyy')  ORDER BY to_char(a.enddate, 'yyyy'))a"; 

                List<MREGender> returnValue = new List<MREGender>(await dbConnection.QueryAsync<MREGender>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<MREGender>> GetMREGenderVN(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select category, qty, color as backgroundColor from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT coalesce(SUM(coalesce(a.qty, 0)),0) qty, 'Nam        ' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "  Union all";
                strQuery = strQuery + "  SELECT coalesce(SUM(coalesce(b.totalaudience,0)),0) qty,'Nữ       ' category FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#FC0703,#2EFC03} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                List<MREGender> returnValue = new List<MREGender>(await dbConnection.QueryAsync<MREGender>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<MREGender>> GetMREChildGenderVN(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "      select category, qty, color as backgroundColor from(";
                strQuery = strQuery + "  SELECT  row_number() over() as id, category, qty FROM(";
                strQuery = strQuery + "      SELECT coalesce(SUM(coalesce(b.malepercentage, 0)),0) qty, 'Trẻ em trai' category   FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + "  Union all";
                strQuery = strQuery + "  SELECT coalesce(SUM(coalesce(b.femalepercentage,0)),0) qty,'Trẻ em gái' category FROM public.mre a left join public.mredetail b on a.mre_guid =b.mre_guid";
                strQuery = strQuery + "  where ( '0'= '" + distict + "' OR a.gazetteer_level3_name = '" + distict + "') ";

                strQuery = strQuery + "  )a)a,";
                strQuery = strQuery + "  (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "  SELECT unnest('{#030DFC,#E203FC} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id ";


                List<MREGender> returnValue = new List<MREGender>(await dbConnection.QueryAsync<MREGender>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<DataClearnceChart>> GetDataClearnceChart(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select array_to_string (array_agg (areasize), ',') as areasize, array_to_string (array_agg (Year), ',') as Year from( ";
                strQuery = strQuery + "select coalesce(areasize,0) as areasize, a.Year  from YearLine a left join ( ";
                strQuery = strQuery + " select CAST(Sum(coalesce(b.areasize,0)) as int)  as areasize, to_char(b.enddate, 'yyyy') as Year  ";
                strQuery = strQuery + " from    hazreduc b ";
                strQuery = strQuery + " where hazreduc_localid like '%CLC%' ";
                strQuery = strQuery + " AND  ( '0'= '" + distict + "' OR gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + " GROUP BY to_char(b.enddate, 'yyyy')  ";
                strQuery = strQuery + " )b on a.Year::text = b.Year  order by a.Year)a";

                List<DataClearnceChart> returnValue = new List<DataClearnceChart>(await dbConnection.QueryAsync<DataClearnceChart>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<DataClearnceChart2>> GetDataClearnceChart2(string distict)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select year,areasize,CAST('#19C204' as varchar) as color  from( ";
                strQuery = strQuery + "select coalesce(areasize,0) as areasize, a.Year  from YearLine a left join ( ";
                strQuery = strQuery + " select CAST(Sum(coalesce(b.areasize,0)) as int)  as areasize, to_char(b.enddate, 'yyyy') as Year  ";
                strQuery = strQuery + " from    hazreduc b ";
                strQuery = strQuery + " where hazreduc_localid like '%CLC%' ";
                strQuery = strQuery + " AND  ( '0'= '" + distict + "' OR gazetteer_level3_name = '" + distict + "') ";
                strQuery = strQuery + " GROUP BY to_char(b.enddate, 'yyyy')  ";
                strQuery = strQuery + " )b on a.Year::text = b.Year  order by a.Year)a";

                List<DataClearnceChart2> returnValue = new List<DataClearnceChart2>(await dbConnection.QueryAsync<DataClearnceChart2>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<GetDataCHA>> GetDataCHA()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "SELECT CAST(sum(areasize) AS int) areasize, (CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END) from    tthdbu_cha a   left join( ";
                strQuery = strQuery + "    select hazard_guid, status_enum, max(data_entry_date) ";
                strQuery = strQuery + "    from  public.task_has_objective, public.task ";
                strQuery = strQuery + "    where public.task.guid = public.task_has_objective.task_guid ";
                strQuery = strQuery + "    group by hazard_guid, status_enum   	   ";
                strQuery = strQuery + "    ) task on task.hazard_guid = a.hazard_guid ";
                strQuery = strQuery + "    GROUP BY(CASE WHEN task.status_enum in ('Suspended','Ongoing','Completed') THEN task.status_enum ELSE status END)";
              
                List<GetDataCHA> returnValue = new List<GetDataCHA>(await dbConnection.QueryAsync<GetDataCHA>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<LinechartCHAByYear>> GetDataCHAByYear()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "select year,areasize  from( ";
                strQuery = strQuery + "  select coalesce(areasize,0) as areasize, a.Year from YearLine a left join(";
                strQuery = strQuery + "  select CAST(Sum(coalesce(b.areasize,0)) as int)  as areasize, to_char(b.status_Changed_date, 'yyyy') as Year";
                strQuery = strQuery + "   from hazard b";
                strQuery = strQuery + "   GROUP BY to_char(b.status_Changed_date, 'yyyy')  ";
                strQuery = strQuery + "   )b on a.Year::text = b.Year  order by a.Year)a";

                List<LinechartCHAByYear> returnValue = new List<LinechartCHAByYear>(await dbConnection.QueryAsync<LinechartCHAByYear>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<LinechartCHAByDistrict>> GetDataCHAByDistrict()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select gazetteername district,areasize,CAST('#FC0602' AS varchar) AS color   from( ";
                strQuery = strQuery + "     select coalesce(areasize,0) as areasize,gazetteername,gazetteer_level3_name,a.gazetteer_localid from(";
                strQuery = strQuery + "     select gazetteer_localid, gazetteername, gazetteer_level3_name from gazetteer where seqno = 3 and gazetteer_level2_name = 'Quang Binh'";
                strQuery = strQuery + "     ) a left join(";
                strQuery = strQuery + "     select CAST(Sum(coalesce(b.areasize, 0)) as int) as areasize, left(gazetteer_level4_localid, 5) localid";
                strQuery = strQuery + "     from hazard b";
                strQuery = strQuery + "     GROUP BY left(gazetteer_level4_localid, 5)";
                strQuery = strQuery + "     )b on a.gazetteer_localid::text = b.localid )a WHERE gazetteer_localid  NOT IN ('40701') ";

                List<LinechartCHAByDistrict> returnValue = new List<LinechartCHAByDistrict>(await dbConnection.QueryAsync<LinechartCHAByDistrict>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<LinechartCHAByDistrict>> GetDataCHAByDistrictEN()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select gazetteer_level3_name district,areasize,CAST('#FC0602' AS varchar) AS color   from( ";
                strQuery = strQuery + "     select coalesce(areasize,0) as areasize,gazetteername,gazetteer_level3_name,a.gazetteer_localid from(";
                strQuery = strQuery + "     select gazetteer_localid, gazetteername, gazetteer_level3_name from gazetteer where seqno = 3 and gazetteer_level2_name = 'Quang Binh'";
                strQuery = strQuery + "     ) a left join(";
                strQuery = strQuery + "     select CAST(Sum(coalesce(b.areasize, 0)) as int) as areasize, left(gazetteer_level4_localid, 5) localid";
                strQuery = strQuery + "     from hazard b";
                strQuery = strQuery + "     GROUP BY left(gazetteer_level4_localid, 5)";
                strQuery = strQuery + "     )b on a.gazetteer_localid::text = b.localid )a WHERE gazetteer_localid  NOT IN ('40701') ";

                List<LinechartCHAByDistrict> returnValue = new List<LinechartCHAByDistrict>(await dbConnection.QueryAsync<LinechartCHAByDistrict>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<LinechartCHAByStatus>> GetDataCHAByStatus()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = "     select gazetteer_level3_name district, SUM(closed) closed, SUM(open) open, CAST('#00FF00' AS varchar) AS color,CAST('#FF0000' as varchar) AS color2 from(";
                strQuery = strQuery + "       select coalesce(open,0) as open, coalesce(closed, 0) as closed, gazetteername,gazetteer_level3_name,a.gazetteer_localid from(";
                strQuery = strQuery + "        select gazetteer_localid, gazetteername, gazetteer_level3_name";
                strQuery = strQuery + "            from gazetteer where seqno = 3 and gazetteer_level2_name = 'Quang Binh'";
                strQuery = strQuery + "        ) a left join(";
                strQuery = strQuery + "        select";
                strQuery = strQuery + "                (CASE WHEN Status_enum <> 'Closed' THEN CAST(Sum(coalesce(b.areasize, 0)) as int) END) as open,";
                strQuery = strQuery + "                (CASE WHEN Status_enum = 'Closed' THEN CAST(Sum(coalesce(b.areasize, 0)) as int) END) as closed, ";
                strQuery = strQuery + "           left(gazetteer_level4_localid, 5) localid";
                strQuery = strQuery + "       from hazard b where isactive = true AND Status_enum IN('Closed', 'Open')";              
                strQuery = strQuery + "       GROUP BY left(gazetteer_level4_localid, 5),Status_enum";
                strQuery = strQuery + "       )b on a.gazetteer_localid::text = b.localid )a  WHERE gazetteer_localid  NOT IN ('40701','40715') GROUP BY gazetteer_level3_name";

                List<LinechartCHAByStatus> returnValue = new List<LinechartCHAByStatus>(await dbConnection.QueryAsync<LinechartCHAByStatus>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

       
        public async Task<List<UXOCategory>> UXOModelSurvey()
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "      select category, qty, color as backgroundColor from( ";
                strQuery = strQuery + "      SELECT  row_number() over() as id, category, qty FROM";
                strQuery = strQuery + "      (SELECT model as category, SUM(qty) qty  FROM public.hazreducdeviceinfo a, hazreduc b";
                strQuery = strQuery + "  where a.hazreduc_guid = b.hazreduc_guid AND resource like 'EV%' AND (b.hazreduc_localid LIKE '%-TS%') ";
                strQuery = strQuery + "    GROUP BY model";
                strQuery = strQuery + "    ORDER BY SUM(qty) DESC LIMIT 5";
                strQuery = strQuery + "    )a)a,";
                strQuery = strQuery + "    (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "    SELECT unnest('{#FC0703,#2EFC03, #815DF6, #67B7DC ,#FDD400} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id";
                List<UXOCategory> returnValue = new List<UXOCategory>(await dbConnection.QueryAsync<UXOCategory>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        public async Task<List<UXOCategory>> UXOModelClearance(string Org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "      select category, qty, color as backgroundColor from( ";
                strQuery = strQuery + "      SELECT  row_number() over() as id, category, qty FROM";
                strQuery = strQuery + "      (SELECT model as category, SUM(qty) qty  FROM public.hazreducdeviceinfo a, hazreduc b";
                strQuery = strQuery + "  where a.hazreduc_guid = b.hazreduc_guid AND resource like 'EV%' AND (b.hazreduc_localid LIKE '%-CLC-%') ";
                strQuery = strQuery + " AND  ( '0'= '" + Org + "' OR org_localid = '" + Org + "') ";
                strQuery = strQuery + "    GROUP BY model";
                strQuery = strQuery + "    ORDER BY SUM(qty) DESC LIMIT 5";
                strQuery = strQuery + "    )a)a,";
                strQuery = strQuery + "    (SELECT row_number() over() as id , color FROM(";
                strQuery = strQuery + "    SELECT unnest('{#FC0703,#2EFC03, #815DF6, #67B7DC ,#FDD400} ' :: TEXT[]) color)b)b";
                strQuery = strQuery + "  WHERE a.id=b.id";
                List<UXOCategory> returnValue = new List<UXOCategory>(await dbConnection.QueryAsync<UXOCategory>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }

        //Get data Clearance
        public async Task<List<LinechartCHAByYear>> GetDataCLCByYear(string Org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();
                string strQuery = "select year,areasize  from( ";
                strQuery = strQuery + "     select coalesce(areasize,0) as areasize, a.Year from YearLine a left join(";
                strQuery = strQuery + "     select CAST(Sum(coalesce(b.areasize,0)) as int)  as areasize, to_char(b.enddate, 'yyyy') as Year";
                strQuery = strQuery + "     from hazreduc b";
                strQuery = strQuery + "     where hazreduc_localid like '%CLC%' ";
                strQuery = strQuery + " AND  ( '0'= '" + Org + "' OR org_localid = '" + Org + "') ";
                strQuery = strQuery + "     GROUP BY to_char(b.enddate, 'yyyy')  ";
                strQuery = strQuery + "     )b on a.Year::text = b.Year  order by a.Year)a";
  
                List<LinechartCHAByYear> returnValue = new List<LinechartCHAByYear>(await dbConnection.QueryAsync<LinechartCHAByYear>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<LinechartCHAByDistrict>> GetDataCLCByDistrict(string Org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select gazetteername district,areasize,CAST('#20AB02' as varchar)  color   from( ";
                strQuery = strQuery + "     select coalesce(areasize,0) as areasize,gazetteername,gazetteer_level3_name,a.gazetteer_localid from(";
                strQuery = strQuery + "     select gazetteer_localid, gazetteername, gazetteer_level3_name from gazetteer where seqno = 3 and gazetteer_level2_name = 'Quang Binh'";
                strQuery = strQuery + "     ) a left join(";
                strQuery = strQuery + "     select CAST(Sum(coalesce(b.areasize, 0)) as int) as areasize, left(gazetteer_level4_localid, 5) localid";
                strQuery = strQuery + "     from hazreduc b";
                strQuery = strQuery + "     where hazreduc_localid like '%CLC%' ";
                strQuery = strQuery + " AND  ( '0'= '" + Org + "' OR org_localid = '" + Org + "') ";
                strQuery = strQuery + "     GROUP BY left(gazetteer_level4_localid, 5)";
                strQuery = strQuery + "     )b on a.gazetteer_localid::text = b.localid )a  ";

                List<LinechartCHAByDistrict> returnValue = new List<LinechartCHAByDistrict>(await dbConnection.QueryAsync<LinechartCHAByDistrict>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
        public async Task<List<LinechartCHAByDistrict>> GetDataCLCByDistrictEN(string Org)
        {
            using (IDbConnection dbConnection = ConnectionStaging)
            {
                dbConnection.Open();

                string strQuery = " 	select gazetteer_level3_name district,areasize,CAST('#20AB02' as varchar) AS color   from( ";
                strQuery = strQuery + "     select coalesce(areasize,0) as areasize,gazetteername,gazetteer_level3_name,a.gazetteer_localid from(";
                strQuery = strQuery + "     select gazetteer_localid, gazetteername, gazetteer_level3_name from gazetteer where seqno = 3 and gazetteer_level2_name = 'Quang Binh'";
                strQuery = strQuery + "     ) a left join(";
                strQuery = strQuery + "     select CAST(Sum(coalesce(b.areasize, 0)) as int) as areasize, left(gazetteer_level4_localid, 5) localid";
                strQuery = strQuery + "     from hazreduc b";
                strQuery = strQuery + "     where hazreduc_localid like '%CLC%' ";
                strQuery = strQuery + " AND  ( '0'= '" + Org + "' OR org_localid = '" + Org + "') ";
                strQuery = strQuery + "     GROUP BY left(gazetteer_level4_localid, 5)";
                strQuery = strQuery + "     )b on a.gazetteer_localid::text = b.localid )a  ";

                List<LinechartCHAByDistrict> returnValue = new List<LinechartCHAByDistrict>(await dbConnection.QueryAsync<LinechartCHAByDistrict>(strQuery));
                dbConnection.Close();

                return returnValue;
            }
        }
    }
}
