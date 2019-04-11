/*
FILE			: Database.cs
PROJECT			: ASQL Kanban Runner :
 PROGRAMMER 	: Michael Ramoutsakis, Billy Parmenter, Michael Ramoutsakis
 FIRST VERSION	: March 14 2018
 DESCRIPTION	: This file holds the constructor and all function definitions
				  for interactions with the database.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Timer = System.Timers.Timer;



namespace KanbanRunner
{
    class Database
    {

        private int timeScale;

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
        public Database()
        {
            Initialize();
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
        private void Initialize()
        {
            string connectionStringSource = ConfigurationManager.ConnectionStrings["TrustedSource"].ConnectionString;

            //start connection to the database
            connectionSource = new SqlConnection(connectionStringSource);

            GetTimeScale();
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

                timeScale = (int)getTimeScale.ExecuteScalar();

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }

        }




        /*
*      FUNCTION : Wait
*      DESCRIPTION :
*          - Wait the five minutes scaaled by the time scale
*      PARAMETERS : 
*          none
*      RETURNS:
*          none    
*/
        public void Wait()
        {
            double time = 30000 / timeScale;

            Thread.Sleep((int)time);
        }




        /*
*      FUNCTION : FillBins
*      DESCRIPTION :
*          - Fill the bins that are in the event table
*      PARAMETERS : 
*          none
*      RETURNS:
*          none    
*/
        public void FillBins()
        {
            string Query = "Fill_Bins";
            SqlCommand fillBins = new SqlCommand(Query, connectionSource)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };



            try
            {
                connectionSource.Open();

                fillBins.ExecuteNonQuery();

                connectionSource.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connectionSource.Close();
            }
        }




        /*
*      FUNCTION : Run
*      DESCRIPTION :
*          - Add the last run to the db
*      PARAMETERS : 
*          none
*      RETURNS:
*          none    
*/
        public void Run()
        {
            string Query = "Insert into [Event] values ('Last_Run', @Unix_Time)";
            SqlCommand run = new SqlCommand(Query, connectionSource);

            int runTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            run.Parameters.AddWithValue("@Unix_Time", runTime);


            try
            {
                connectionSource.Open();

                run.ExecuteNonQuery();

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
