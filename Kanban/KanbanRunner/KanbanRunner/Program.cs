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
            Database db = new Database();

            while (true)
            {
                db.Run();
                db.Wait();
                db.FillBins();
            }
        }
    }
}
