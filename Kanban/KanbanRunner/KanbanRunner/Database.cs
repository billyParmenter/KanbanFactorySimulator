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
    /*
     * Handles database interaction
     */
    class Database : IDisposable
    {
        /*
         * The current simulation timescale
         */
        public int TimeScale => _timeScale;
        
        /*
         * If this database object is connected
         */
        public bool IsConnected { get; private set; } = false;
        
        /*
         * The last error that occured (only set during intialization)
         */
        public SqlException LastError { get; private set; } = null;

        private int _timeScale, _runnerDelay_Seconds;

        private SqlConnection connectionSource;

        private Configuration configuration;




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

            try
            {
                connectionSource.Open();
                IsConnected = true;
            }
            catch(SqlException ex)
            {
                IsConnected = false;
                LastError = ex;
            }

            // only create a configuration and get its values if the database
            // is connected
            if (IsConnected)
            {
                configuration = new Configuration(connectionSource);

                GetTimeScale();

                GetRunnerDelay();
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
        *      THROWS:
        *          InvalidOperationException if Time_Scale doesn't exist or isn't an int
        */
        private void GetTimeScale()
        {
            if(!configuration.GetInteger("Time_Scale", out _timeScale))
            {
                throw new InvalidOperationException("Could not get Time_Scale (int) from configuration table");
            }
        }


        /*
         * FUNCTION: GetRunnerDelay
         * DESCRIPTION:
         *      - Gets the runner delay from the config table
         * PARAMETERS:
         *      none
         * RETURNS:
         *      none
        *      THROWS:
        *          InvalidOperationException if Runner_Delay doesn't exist or isn't an int
         */
        private void GetRunnerDelay()
        {
            if (!configuration.GetInteger("Runner_Delay", out _runnerDelay_Seconds))
            {
                throw new InvalidOperationException("Could not get Runner_Delay (int) from configuration table");
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
            double time = _runnerDelay_Seconds / _timeScale;

            // time is in seconds, sleep takes milliseconds
            Thread.Sleep((int)(time * 1000));
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
                CommandType = CommandType.StoredProcedure
            };

            fillBins.ExecuteNonQuery();
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
            string Query = "Update_Runner_Time";
            SqlCommand run = new SqlCommand(Query, connectionSource)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            run.ExecuteNonQuery();
        }

        /*
         * DESCRIPTION:
         *      Closes the sql connection
         */
        public void Dispose()
        {
            connectionSource.Close();
            IsConnected = false;
        }
    }
}
