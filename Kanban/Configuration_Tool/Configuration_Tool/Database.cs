using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Configuration_Tool
{
    class Database
    {

        private readonly string dbName = "kanban";
        private SqlConnection sqlConnection;



        public Database()
        {
            Initialize();
        }





        /*
                 -------Function-------

             Name  : Initialize
             Info  : Initializes the database connection
             Params: none
             Return: none

        */
        private void Initialize()
        {
            string connectionString = string.Format(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString, dbName);

            //start connection to the database
            sqlConnection = new SqlConnection(connectionString);
        }





        /*
                -------Function-------

            Name  : GetConfigValues
            Info  : Gets the current config values from the database
            Params: none
            Return: A dictionary of config values (< configKey, configValue>)

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

                while (reader.Read())
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

            Name  : GetLanes
            Info  : Gets the current lanes from the database
            Params: none
            Return: A dictionary of lanes from the database (< laneID, workerExpLevel>)

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

            Name  : UpdateDatabase
            Info  : Updates the database with the new configuration values and lanes
            Params: configValues - A dictionary of config values to update
                    lanes - A dictionary of lanes to update
                    lanesFromDB - The number of lanes in the database
            Return: none

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

            Name  : AddLanes
            Info  : Adds lanes to the database
            Params: lanes - A dictionary of lanes to add to the database
            Return: none

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

            Name  : UpdateLanes
            Info  : Updates the worker experience level of each lane in the database
            Params: lanes - The dictionary of lanes (< laneID, workerExpLevel>)
                    lanesFromDB - the number of lanes in the database
            Return: none

        */
        private void UpdateLanes(Dictionary<int, int> lanes, int lanesFromDB)
        {
            for (int i = 0; i < lanesFromDB; i++)
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

            Name  : Update
            Info  : Updates a config value 
            Params: key - The key of the config to update
                    value - The value to set the config to
            Return: none

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
