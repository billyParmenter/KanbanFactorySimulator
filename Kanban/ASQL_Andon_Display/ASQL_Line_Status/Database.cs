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

namespace ASQL_Line_Status
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
            using (SqlCommand cmd = new SqlCommand("SELECT LaneID FROM Lane", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Lanes.Clear();
                    foreach (var i in reader)
                    {
                        Lanes.Add(reader.GetInt32(0));
                    }
                }
            }
        }
        
        private void GetLaneInformation(int LaneID, DataTable table)
        {
            using (SqlCommand cmd = new SqlCommand("Get_Lane_Statistics", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LaneID", LaneID);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    table.Clear();
                    adapter.Fill(table);
                }
            }
        }

        public void GetLaneInformation(DataTable table)
        {
            UpdateLanes();

            table.Clear();
            table.Columns.Clear();

            DataColumn lanecol = table.Columns.Add("Lane ID");
            DataColumn goodcol = table.Columns.Add("Good Parts");
            DataColumn badcol = table.Columns.Add("Bad Parts");
            DataColumn yieldcol = table.Columns.Add("% Yield");

            DataTable temp = new DataTable();

            int good_parts = 0,
                bad_parts = 0;

            foreach (int lane in Lanes)
            {
                temp.Clear();
                GetLaneInformation(lane, temp);

                foreach(DataRow temp_row in temp.Rows)
                {
                    DataRow row2 = table.Rows.Add();

                    row2[lanecol] = lane;
                    row2[goodcol] = temp_row["Good Parts"];
                    row2[badcol] = temp_row["Bad Parts"];
                    row2[yieldcol] = Convert.ToDouble(temp_row["% Yield"]) * 100 + "%";

                    good_parts += Convert.ToInt32(row2[goodcol]);
                    bad_parts += Convert.ToInt32(row2[badcol]);
                }
            }
            
        }

    }
    
}
