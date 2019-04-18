using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ASQL_Andon_Display
{
    /// <summary>
    /// Gathers information about a specific lane.
    /// </summary>
    public class Database
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// The lane list.
        /// </summary>
        public List<int> Lanes { get; } = new List<int>();
        
        /// <summary>
        /// The current lane.
        /// </summary>
        public int CurrentLane { get; set; }

        /// <summary>
        /// The bins for the current lane.
        /// </summary>
        public DataTable Bins { get; } = new DataTable();
        
        public Database()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            
            connection.Open();
            
            UpdateLanes();
        }
        
        /// <summary>
        /// Updates the lane list.
        /// </summary>
        public void UpdateLanes()
        {
            SqlCommand cmd = new SqlCommand("SELECT LaneID FROM Lane", connection);

            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                Lanes.Clear();
                foreach(var i in reader)
                {
                    Lanes.Add(reader.GetInt32(0));
                }
            }
        }

        /// <summary>
        /// Updates the bins.
        /// </summary>
        public void UpdateBins()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT [Harness_Count], [Reflector_Count], [Housing_Count]," +
                "[Lens_Count], [Bulb_Count], [Bezel_Count] FROM Lane WHERE LaneID = @laneid", connection))
            {
                cmd.Parameters.AddWithValue("@laneid", CurrentLane);
                
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);

                    Bins.Clear();
                    Bins.Columns.Clear();

                    DataColumn name = Bins.Columns.Add("Bin Name"),
                               counts = Bins.Columns.Add("Part Count");

                    foreach(DataColumn col in temp.Columns)
                    {
                        DataRow bin = Bins.Rows.Add();
                        bin[name] = col.ColumnName;

                        // for every row in the table, sum the values in the current column
                        bin[counts] = temp.Rows.OfType<DataRow>()
                            .Select(row => Convert.ToInt32(row[col]))
                            .Sum();
                    }
                }
            }
        }

        /// <summary>
        /// Checks the current item's assembly progress
        /// </summary>
        /// <returns>The progress with range 0 to 1</returns>
        public double GetAssemblyProgress()
        {
            using(SqlCommand cmd = new SqlCommand("dbo.Get_Remaining_Time", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LaneID", CurrentLane);

                if (double.TryParse(cmd.ExecuteScalar().ToString(), out double result))
                {
                    return result > 1 ? 1 : result;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Checks the runner's remaining time to check.
        /// </summary>
        /// <returns>The remaining time with range 0 to 1</returns>
        public double GetRunnerProgress()
        {
            using (SqlCommand cmd = new SqlCommand("dbo.Get_Runner_Time", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (double.TryParse(cmd.ExecuteScalar().ToString(), out double result))
                {
                    return result > 1 ? 1 : result;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Checks if any bins are lower than the runner threshold.
        /// </summary>
        public bool IsRunnerRequired()
        {
            int threshold = GetRunnerThreshold();

            foreach (DataRow row in Bins.Rows)
            {
                if(Convert.ToInt32(row["Part Count"]) <= threshold)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the runner threshold from the config table.
        /// </summary>
        private int GetRunnerThreshold()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT [Value] FROM Config WHERE [Key] = 'RunnerThreshold'", connection))
            {
                if(!int.TryParse(cmd.ExecuteScalar().ToString(), out int result))
                {
                    throw new InvalidOperationException("Database Configuration Error: Runner Threshold is not an integer");
                }

                return result;
            }
        }

    }
    
}
