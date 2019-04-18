/*
FILE			: Program.cs
PROJECT			: ASQL Kanban Simulation :
 PROGRAMMER 	: Michael Ramoutsakis, Billy Parmenter, Michael Ramoutsakis
 FIRST VERSION	: March 14 2018
 DESCRIPTION	: This file holds runs the simulation of the 
                  kanban worker
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ASQL_Simulation
{
    class Program
    { 
        static void Main(string[] args)
        {
            int exit = -1;

            //
            Lane lane = new Lane();


            // Initialize the database connection and get the lanes from the databse
            Database db = new Database(lane);
            Dictionary<int, int> lanes = db.GetLanes();


            // Display the lanes to the user and ask what lane to start
            DisplayLanes(lanes);
            lane.LaneID = GetInput(lanes);


            // If GetInput returns -1 then exit
            if (lane.LaneID != exit)
            {
                Sim sim = new Sim();

                Thread thread = new Thread(()=>sim.RunSim(db, lane));

                thread.Start();
                
                Console.Write("Press any key to stop...");

                Console.ReadKey();

                thread.Interrupt();

                db.ReleaseLane();
            }
        }





        /*
                -------Function-------

            Name  : DisplayLanes
            Info  : Displays the lanes that are currently stored in the database
            Params: Dictionary<int, int> lanes - The lanes to display
            Return: None

        */
        public static void DisplayLanes(Dictionary<int, int> lanes)
        {
            Console.WriteLine("Choose a lane to start running:");

            foreach (KeyValuePair<int, int> lane in lanes)
            {
                string exp;
                if (lane.Value == 1)
                {
                    exp = "a new worker";
                }
                else if (lane.Value == 2)
                {
                    exp = "an experienced worker";
                }
                else
                {
                    exp = "a super worker";
                }

                Console.WriteLine("Lane: " + lane.Key + " has " + exp);

            }
        }





        /*
                -------Function-------

            Name  : GetInput
            Info  : Gets the useres choice of lane
            Params: Dictionary<int, int> lanes - The lanes to choose from
            Return: Int - The chosen lane or -1 if exit

        */
        public static int GetInput(Dictionary<int, int> lanes)
        {
            Console.Write("Start lane:");
            bool gettingInput = true;
            int retLane = 0;

            while (gettingInput == true)
            {
                string userInPut = Console.ReadLine();
                int laneID = -1;

                if (int.TryParse(userInPut, out laneID))
                {
                    if (lanes.ContainsKey(laneID))
                    {
                        retLane = laneID;
                        gettingInput = false;
                    }
                    else
                    {
                        Console.WriteLine("invalid");
                    }
                }
                else
                {
                    if (userInPut == "exit")
                    {
                        retLane = -1;
                        gettingInput = false;
                    }
                    else
                    {
                        Console.WriteLine("invalid");
                    }
                }
            }

            return retLane;
        }
    }
}
