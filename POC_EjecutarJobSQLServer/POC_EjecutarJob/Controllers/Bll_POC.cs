using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POC_EjecutarJob.Controllers
{
    public class Bll_POC
    {
        private string connectionString = "data source=PDP033;initial catalog=TESIS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";


        public List<string> listarJob(string DataBaseName)
        {
            try
            {
                SqlConnection Con = new SqlConnection(connectionString);
                SqlCommand ExeJob = new SqlCommand();

                ExeJob.CommandType = CommandType.StoredProcedure;
                ExeJob.CommandText = "msdb.dbo.sp_start_job"; 
                string SQL = $"SELECT job.job_id, notify_level_email, name, enabled, description, step_name, command, server, database_name FROM msdb.dbo.sysjobs job INNER JOIN msdb.dbo.sysjobsteps steps ON job.job_id = steps.job_id WHERE job.enabled = 1 and database_name ='{DataBaseName}' order by name";

				// Este es el Query para saber si un Job aun esta ejecutandose 
				//SELECT 1 FROM msdb.dbo.sysjobs J 
				//JOIN msdb.dbo.sysjobactivity A ON A.job_id = J.job_id WHERE J.name = 'bonos_CargueBonos' 
				//AND A.run_requested_date IS NOT NULL AND A.stop_execution_date IS NULL

                List<string> Lista = new List<string>();

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand(SQL, cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(Convert.ToString(dr["name"]));
                        Console.WriteLine(Convert.ToString(dr["name"]));
                    }
                    dr.Close();
                }

                return Lista;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public List<string> listarBD()
        {
            try
            {
                SqlConnection Con = new SqlConnection(connectionString);
                SqlCommand ExeJob = new SqlCommand();

                string SQL = @"SELECT name, database_id, create_date FROM sys.databases";


                List<string> Lista = new List<string>();

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand(SQL, cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(Convert.ToString(dr["name"]));
                    }
                    dr.Close();
                }

                return Lista;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }


        public bool EjecutarJob(string NombreJob)
        {
            try
            {
                SqlConnection Con = new SqlConnection(connectionString);
                SqlCommand ExeJob = new SqlCommand();

                ExeJob.CommandType = CommandType.StoredProcedure;
                ExeJob.CommandText = "msdb.dbo.sp_start_job";
                ExeJob.Parameters.AddWithValue(@"job_name", NombreJob);
                ExeJob.Connection = Con;

                using (Con)
                {
                    Con.Open();

                    using (ExeJob)
                    {
                        ExeJob.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
        }


    }
}