# KanbanFactorySimulator
Simulates a kanban based factory system.

The system can simulate three types of employees, new, regular and super.Each with a varying speed and rate of failure (bad part). The system will create a lane for each employee to work at with a given number of parts. Part inventory is managed by a SQL Server database. When a lanes inventory goes below a certain threshold a "runner" is notified. Once the runners rest time is up it will replenish all parts in all lanes that reqire restocking. The runner will again enter rest. As the lanes create parts they are tested and added to trays.

# To run:
  1) Run KanbanDbSetup.sql in SQL Server</br>
  2) Run Config_Tool.exe and add lanes</br>
  3) Run KanbanRunner.exe</br>
  4) Run Simulation.exe for each lane</br>
