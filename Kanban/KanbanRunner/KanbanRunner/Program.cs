/*
FILE			: Program.cs
PROJECT			: ASQL Kanban Simulation :
 PROGRAMMER 	: Michael Ramoutsakis, Billy Parmenter, Michael Ramoutsakis
 FIRST VERSION	: March 14 2018
 DESCRIPTION	: This file holds runs the simulation of the 
                  kanban runner
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger("KANBAN-RUNNER");
            
            // The using(...) & extending IDisposable will call Dispose() on db
            // automatically when the scope is exited (on error, or on success)
            using (Database db = new Database()) {

                // if the database isn't connected, print the error and exit
                if(!db.IsConnected)
                {
                    logger.Log("Could not connect to database:");
                    logger.Log(db.LastError.Message);
                    return;
                }

                logger.Log("Connected to database");
                logger.Log("Time Scale: {0}", db.TimeScale);

                // Update the current run, wait the correct amount of time, and
                // update the bins
                while (true)
                {
                    db.Run();
                    logger.Log("Updated last run, waiting for next run");

                    db.Wait();
                    logger.Log("Done waiting, refilling bins");

                    db.FillBins();
                    logger.Log("Done filling bins");
                }
            }
        }
    }
}
