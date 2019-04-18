using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASQL_Simulation
{
    class Sim
    {
        /// <summary>
        /// Constructs lamps indefinitely.
        /// </summary>
        /// <param name="db">The database to interact with</param>
        /// <param name="lane">The lane to construct parts on</param>
        public void RunSim(Database db, Lane lane)
        {
            Logger logger = new Logger("SIMULATOR-" + lane.LaneID);

            db.GetWorkerInfo();
            
            while (true)
            {
                db.CreatePart(lane.LaneID);

                logger.Log("Created new part");
                
                int units_tested = db.TestProduct(lane.LaneID);

                logger.Log("Tested {0} units", units_tested);

                int sleep_time = lane.CalcTimeDev();

                db.UpdateNextFinish(sleep_time / 1000.0);
                logger.Log("Sleeping for {0} seconds to create new part", sleep_time / 1000.0);

                Thread.Sleep(sleep_time);
            }

        }
    }
}
