using System;

namespace ASQL_Simulation
{
    class Lane
    {
        public int WorkerID { get; set; }
        public double TimeDev { get; set; }
        public double FailRate { get; set; }
        public int TimeScale { get; set; }
        public int LaneID { get; set; }





        /*
        *      FUNCTION : calcTimeDev
        *      DESCRIPTION :
        *          - This function will calculate the time
        *          deviation to be used in the workstation
        *          based on the time deviation value from the 
        *          worker.
        *      PARAMETERS : 
        *          none
        *      RETURNS:
        *          none    
        */
        public int CalcTimeDev()
        {
            double time = 60000 / TimeScale;

            Random random = new Random();
            if (random.Next(0, 1) == 0)
            {
                time *= 1.10;
            }
            else
            {
                time *= .9;
            }
            if (TimeDev != 0)
            {
                time *= TimeDev;
            }

            return (int)time;
        }

        
    }
}
