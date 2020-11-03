# KanbanFactorySimulator
Simulates a kanban based factory system.

The system is a database-backed simulation to support and test a simple
electronic Kanban solution.

The factory builds a fog lamp assembly for automobiles. The assembly is made up of a harness, a
reflector, a housing, a lens, a bulb and a bezel. Each lane has its own inventory of these parts. An employee takes
one of each of these parts and puts it together. Once completed, the fog lamp is placed on a tray with
separators. When full, the tray is sent to the testing department.

The lanes, parts, and trays are all tracked in the database. When a lane completes a part, the inventory for the lane is updated. 
There is an event table in the database that keeps track of bins at the lanes that need to be refilled. 
When there are only 5 parts left, the bin and lane are added to the event table. A runner program looks at the event table every 5 minutes by default. 
It goes to each lane and fills the part bins that need to be refilled.

There is a display to show the status of the entire assembly line in real time. This is in the
form of a conventional Kanban display with Order amount, in process amount, number
produced and yield.

There are three worker types that a lane can have, new, experienced, and super worker. Each worker has a different base speed and failure rate. 
When simulating a time deviency is calculated based on experience level for each part being made.


# To run:
  1) Run KanbanDbSetup.sql in SQL Server to setup the database</br>
  2) Run Config_Tool.exe to add lanes and adjust the configuration</br>
  3) Run KanbanRunner.exe</br>
  4) Run Simulation.exe for each lane to start making parts</br>
  5) Run ASQL_Andon_Display to view the progress of the lanes and runner</br>
  6) Run ASQL_Line_Status to view the number of good and bad parts along with yild for each lane</br>
