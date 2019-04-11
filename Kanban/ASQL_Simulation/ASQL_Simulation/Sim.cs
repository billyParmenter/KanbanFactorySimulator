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
        public bool DoProcessing = true;

        public void RunSim(Database db, Lane lane)
        {
            db.GetWorkerInfo();

            int Pass;

            while (DoProcessing)
            {
                db.CreatePart(lane.LaneID);

                Pass = lane.CalcFailRate();

                db.TestProduct(Pass, lane.WorkerID);

                //Console.WriteLine("Working...");
                Thread.Sleep(lane.CalcTimeDev());
            }

            db.ReleaseLane();
        }
    }
}
