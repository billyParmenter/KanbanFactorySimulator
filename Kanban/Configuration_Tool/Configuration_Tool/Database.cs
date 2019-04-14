using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Configuration_Tool
{
    class Database
    {

        private readonly string dbName = "kanban";
        private SqlConnection sqlConnection;





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        public Database()
        {
            Initialize();
        }





        /*
                 -------Function-------

             Name  : 
             Info  : 
             Params: 
             Return: 

        */
        private void Initialize()
        {
            string connectionString = string.Format(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString, dbName);

            //start connection to the database
            sqlConnection = new SqlConnection(connectionString);
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        public Dictionary<string, int> GetConfigValues()
        {
            Dictionary<string, int> configValues = new Dictionary<string, int>();

            string query = string.Format("SELECT [key], [value] FROM Config");
            SqlCommand command = new SqlCommand(query, sqlConnection);



            try
            {
                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    configValues.Add(reader.GetString(0), int.Parse(reader.GetString(1)));
                }

                sqlConnection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return configValues;
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        public Dictionary<int, int> GetLanes()
        {
            Dictionary<int, int> lanes = new Dictionary<int, int>();

            string query = string.Format("select laneid, Worker.Experience_Level from lane inner join worker on Lane.WorkerID = Worker.WorkerID");
            SqlCommand command = new SqlCommand(query, sqlConnection);



            try
            {
                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lanes.Add(reader.GetInt32(0), reader.GetInt32(1));
                }

                sqlConnection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return lanes;
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        public void UpdateDatabase(Dictionary<string, int> configValues, Dictionary<int, int> lanes, int lanesFromDB)
        {
            Update("Time_Scale", configValues["Time_Scale"]);
            Update("Harness_Count", configValues["Harness_Count"]);
            Update("Reflector_Count", configValues["Reflector_Count"]);
            Update("Housing_Count", configValues["Housing_Count"]);
            Update("Lens_Count", configValues["Lens_Count"]);
            Update("Bulb_Count", configValues["Bulb_Count"]);
            Update("Bezel_Count", configValues["Bezel_Count"]);
            Update("Test_Tray_Cap", configValues["Test_Tray_Cap"]);
            Update("Runner_Delay", configValues["Runner_Delay"]);

            UpdateLanes(lanes, lanesFromDB);

            for (int i = 1; i <= lanesFromDB; i++)
            {
                lanes.Remove(i);
            }

            AddLanes(lanes);
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void AddLanes(Dictionary<int, int> lanes)
        {
            int i = 0;

            foreach (var lane in lanes)
            {
                string query = "setup_lane @laneID, @workerExp";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                command.Parameters.AddWithValue("@laneID", lanes.ElementAt(i).Key);
                command.Parameters.AddWithValue("@workerExp", lanes.ElementAt(i).Value);

                try
                {
                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    sqlConnection.Close();
                }

                i++;
            }
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void UpdateLanes(Dictionary<int, int> lanes, int lanesFromDB)
        {
            for(int i = 0; i < lanesFromDB; i++)
            {
                string query = "update worker set Experience_Level = @Exp where WorkerID = (select WorkerID from lane where LaneID = @LaneID)";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                command.Parameters.AddWithValue("@Exp", lanes.ElementAt(i).Value);
                command.Parameters.AddWithValue("@LaneID", lanes.ElementAt(i).Key);

                try
                {
                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    sqlConnection.Close();
                }

                i++;
            }
        }





        /*
                -------Function-------

            Name  : 
            Info  : 
            Params: 
            Return: 

        */
        private void Update(string key, int value)
        {
            string query = "Update Config set [value] = @value where [key] = @key";
            SqlCommand command = new SqlCommand(query, sqlConnection);

            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@key", key);

            try
            {
                sqlConnection.Open();

                command.ExecuteNonQuery();

                sqlConnection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                sqlConnection.Close();
            }
        }
    }
}
