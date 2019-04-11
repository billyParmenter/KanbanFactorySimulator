/*
FILE			: Database.cs
PROJECT			: ASQL Kanban Simulation :
 PROGRAMMER 	: Michael Ramoutsakis, Billy Parmenter, Michael Ramoutsakis
 FIRST VERSION	: March 14 2018
 DESCRIPTION	: This file holds the constructor and all function definitions
				  for interactions with the database.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASQL_Simulation
{
    class Database
    {
        Lane lane;



        private SqlConnection connectionSource;
        /*
        *      FUNCTION : Database
        *      DESCRIPTION :
        *          - Constructor for the database, calls
        *            initialize function.
        *            class.
        *      PARAMETERS :
        *          none
        *      RETURNS:
        *          none    
        */
        public Database(Lane lane)
        {
            Initialize(lane);
        }





        /*
        *      FUNCTION : Initialize
        *      DESCRIPTION :
        *          - This function connects to the database
        *            through the Default connection string on the local host
        *            connection string found in App.config.
        *      PARAMETERS :
        *          none
        *      RETURNS:
        *          none    
        */
        private void Initialize(Lane lane)
        {
            string connectionStringSource = ConfigurationManager.ConnectionStrings["TrustedSource"].ConnectionString;

            this.lane = lane;

            //start connection to the database
            connectionSource = new SqlConnection(connectionStringSource);

            GetTimeScale();
        }





        /*
        *      FUNCTION : getID
        *      DESCRIPTION :
        *          - This function will call a stored procedure
        *            in the database to get the laneID
        *      PARAMETERS : 
        *          none
        *      RETURNS:
        *          none    
        */
        public int GetID()
        {
            string Query = "Choose_Lane";
            int laneID = 0;
            SqlCommand getLane = new SqlCommand(Query, connectionSource)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            getLane.Parameters.Add("@RetLane", SqlDbType.Int).Direction = ParameterDirection.Output;
           
            try
            {
                connectionSource.Open();

                getLane.ExecuteNonQuery();
                laneID = (int)getLane.Parameters["@RetLane"].Value;

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

            return laneID;
        }





        /*
        *      FUNCTION : getWorkerInfo
        *      DESCRIPTION :
        *          - This function will use a stored procedure
        *          in the database in order to get the workers
        *          ID, time deviation and fail rate based on 
        *          LaneID
        *      PARAMETERS : 
        *          int LaneID
        *      RETURNS:
        *          int experienceLevel;    
        */
        public void GetWorkerInfo()
        {
            string Query = "Get_WorkerInfo";
            SqlCommand getWorkerInfo = new SqlCommand(Query, connectionSource);
            SqlDataReader myReader;
            getWorkerInfo.CommandType = System.Data.CommandType.StoredProcedure;

            getWorkerInfo.Parameters.AddWithValue("@LaneID", lane.LaneID);

            try
            {
                connectionSource.Open();
                myReader = getWorkerInfo.ExecuteReader();


                while (myReader.Read())
                {
                    lane.WorkerID = myReader.GetInt32(0);
                    lane.TimeDev = myReader.GetDouble(1);
                    lane.FailRate = myReader.GetDouble(2);
                }

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

            
        }





        /*
        *      FUNCTION : calcTimeDev
        *      DESCRIPTION :
        *          - This function will call a stored
        *          procedure in the database to create a 
        *          fog light base on LaneID
        *          user.
        *      PARAMETERS : 
        *          int LaneID
        *      RETURNS:
        *          none    
        */
        public void CreatePart(int LaneID)
        {
            string Query = "Part_Created";
            SqlCommand getLane = new SqlCommand(Query, connectionSource)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            getLane.Parameters.AddWithValue("@LaneID", LaneID);

            try
            {
                connectionSource.Open();

                getLane.ExecuteNonQuery();

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

        }





        /*
        *      FUNCTION : TestProduct
        *      DESCRIPTION :
        *          - This function will update the
        *          Test_Unit table with tested item values
        *          user.
        *      PARAMETERS : 
        *          int fail, int workerID
        *      RETURNS:
        *          none    
        */
        public void TestProduct(int fail, int workerID)
        {
            string Query = "Test_Product";
            SqlCommand getLane = new SqlCommand(Query, connectionSource)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            getLane.Parameters.AddWithValue("@Fail", fail);
            getLane.Parameters.AddWithValue("@workerID", workerID);

            try
            {
                connectionSource.Open();

                getLane.ExecuteNonQuery();

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

        }





        /*
*      FUNCTION : GetTimeScale
*      DESCRIPTION :
*          - This function will get the time scale form the config table
*      PARAMETERS : 
*          none
*      RETURNS:
*          none    
*/
        public void GetTimeScale()
        {

            string Query = "Select [Value] from Config where [Key] = 'Time_Scale'";
            SqlCommand getTimeScale = new SqlCommand(Query, connectionSource);

            try
            {
                connectionSource.Open();

                lane.TimeScale = (int)getTimeScale.ExecuteScalar();

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

        }





        /*
                -------Function-------

            Name  : GetLanes
            Info  : Gets lane ids and stores them into a dictonary as a key,
                        The value of the key is the associated workers experience level
            Params: None
            Return: Dictionary<int, int> - The lane and worker experience

        */
        public Dictionary<int, int> GetLanes()
        {
            Dictionary<int, int> lanes = new Dictionary<int, int>();

            string query = string.Format("select laneid, Worker.Experience_Level, Is_Running from lane inner join worker on Lane.WorkerID = Worker.WorkerID");
            SqlCommand command = new SqlCommand(query, connectionSource);



            try
            {
                connectionSource.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetInt32(2) == 0)
                    {
                        lanes.Add(reader.GetInt32(0), reader.GetInt32(1));
                    }
                }

                connectionSource.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

            return lanes;
        }





        /*
        -------Function-------

    Name  : GetLanes
    Info  : Gets lane ids and stores them into a dictonary as a key,
                The value of the key is the associated workers experience level
    Params: None
    Return: Dictionary<int, int> - The lane and worker experience

*/
        public void ReleaseLane()
        {
            string query = string.Format("Update Lane Set Is_Running = 0 Where LaneID = @LaneID");
            SqlCommand command = new SqlCommand(query, connectionSource);

            command.Parameters.AddWithValue("@LaneID", lane.LaneID);


            try
            {
                connectionSource.Open();

                command.ExecuteNonQuery();

                connectionSource.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }
        }
    }
}
