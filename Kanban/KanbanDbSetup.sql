
use master;

--Drop if existing to clean
IF DB_ID('kanban') IS NOT NULL
Begin
	ALTER DATABASE kanban SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE kanban;
End
go

create database kanban;
go
use kanban;
go

--create the configuration table
Create table Config (
	[Key] varchar(30),
	[Value] varchar(256)
)

insert into Config 
	([key], [value])
Values
	('Time_Scale', '1'),
	('RunnerThreshold', '5'),
	-- delay between runner checks in seconds
	('Runner_Delay', '300'),
	('Harness_Count', '55'),
	('Reflector_Count', '35'),
	('Housing_Count', '24'),
	('Lens_Count', '40'),
	('Bulb_Count', '60'),
	('Bezel_Count', '75'),
	('Test_Tray_Cap', '60');


Create Table [Event](
	[Key] varchar(30),
	[Value] sql_variant
)


--Create the experience table
create table Experience (
	Experience_Level int NOT NULL IDENTITY PRIMARY KEY,
	Time_Deviancy float NOT NULL,
	Fail_Rate float NOT NULL
)



--Create the Worker table
create table Worker (
	WorkerID int NOT NULL IDENTITY PRIMARY KEY,
	Experience_Level int  NOT NULL FOREIGN KEY REFERENCES Experience (Experience_Level)
)



--Create the lane table
create table Lane (
	LaneID int NOT NULL PRIMARY KEY,
	WorkerID int NOT NULL FOREIGN KEY REFERENCES Worker(WorkerID),
	Harness_Count int,
	Reflector_Count int,
	Housing_Count int,
	Lens_Count int,
	Bulb_Count int,
	Bezel_Count int,
	Is_Running int default 0,
	Start_Time datetime,
	End_Time datetime
);



--Create the test_tray table
create table Test_Tray (
	Test_TrayID int NOT NULL IDENTITY PRIMARY KEY,
	LaneID int NOT NULL FOREIGN KEY REFERENCES Lane(LaneID),
	Capacity int NOT NULL Default 60,
	Part_Count int NOT NULL Default 0
)



--create the test_unit table
create table Test_Unit (
	Tray_Position int NOT NULL,
	Test_TrayID int NOT NULL FOREIGN KEY REFERENCES Test_Tray(Test_TrayID),
	WorkerID int NOT NULL FOREIGN KEY REFERENCES Worker(WorkerID),
	Tested bit NOT NULL default 0,
	Test_Result bit NOT NULL default 0,
	
	PRIMARY KEY NONCLUSTERED(Tray_Position, Test_TrayID)
)
go



--Inster the experience levels into the experience table
Insert into Experience (Time_Deviancy, Fail_rate)
Values (1.5, 0.85);

Insert into Experience (Time_Deviancy, Fail_rate)
Values (1, 0.5);

Insert into Experience (Time_Deviancy, Fail_rate)
Values (.85, 0.15);

go


-- Easy way to get information about a lane's worker
CREATE OR ALTER VIEW [LaneWorkerInfo]
AS		SELECT Lane.LaneID, Lane.WorkerID, Experience.Time_Deviancy, Experience.Fail_Rate
	FROM Lane
	INNER JOIN Worker
	ON Lane.WorkerID = Worker.WorkerID
	INNER JOIN Experience
	ON Experience.Experience_Level = Worker.Experience_Level
GO


--- An easy way to get information about a unit
CREATE OR ALTER VIEW [UnitInfo]
AS		SELECT Tray_Position as 'Position', Test_Tray.Test_TrayID as 'TrayID', Lane.LaneID, Worker.WorkerID,
		Tested as 'UnitTested', Test_Tray.Part_Count as 'PartsInTray', Test_Tray.Capacity as 'TrayCapacity'
	FROM Test_Unit
	INNER JOIN Test_Tray
	ON Test_Tray.Test_TrayID = Test_Unit.Test_TrayID
	INNER JOIN Lane
	ON Lane.LaneID = Test_Tray.LaneID
	INNER JOIN Worker
	ON Worker.WorkerID = Lane.WorkerID
GO



--Procedure to create a worker with the given experience
create or alter Procedure Create_Worker
	@Exp int
As
Begin
	If @Exp >= 1 and @Exp <= 3
	Begin

		Insert into Worker (Experience_Level) Values (@Exp)
		Return 0

	End

	Else
		Return 1
End
go



--Create a lane operated by a given worker
Create or alter Procedure Create_Lane
	@laneID int,
	@WorkerID int
As
Begin
	Insert into Lane (LaneID, WorkerID, Harness_Count, Reflector_Count, Housing_Count, Lens_Count, Bulb_Count, Bezel_Count)
	values (@laneID,
			@WorkerID, 
			(select ([value]) from config where [key] = 'Harness_Count'),
			(select ([value]) from config where [key] = 'Reflector_Count'),
			(select ([value]) from config where [key] = 'Housing_Count'),
			(select ([value]) from config where [key] = 'Lens_Count'),
			(select ([value]) from config where [key] = 'Bulb_Count'),
			(select ([value]) from config where [key] = 'Bezel_Count'))
End
go



--Initial setup of all lanes, gives them an inital tray
Create Procedure Setup_Lane
	@laneID int,
	@WorkerExp int
As
	declare @workerID int
Begin

	Exec Create_Worker @WorkerExp
	select top 1 @workerID = workerID from worker order by WorkerID desc
	Exec Create_Lane @laneID, @workerID

	Insert into Test_Tray (LaneID, Capacity)
	Select @laneID, [value]
	From Config
	where [key] = 'Test_Tray_Cap'
		
End
go

--Chose lane to run
Create Procedure Choose_Lane
	@RetLane int OUTPUT
As
	
Begin
	Select Top 1 @RetLane = LaneID From Lane Where Is_Running = 0
	Update Lane Set Is_Running = 1 Where LaneID = @RetLane
	Return @RetLane
End
Go

--Get Worker ID
Create Procedure Get_WorkerInfo
	@LaneID int
As
	
Begin
	Select WorkerID, Time_Deviancy, Fail_Rate from LaneWorkerInfo Where LaneID = @LaneID
End
Go



-- Tests all untested units in full trays for the current lane
CREATE OR ALTER PROCEDURE Do_Tray_Test
	@LaneID INT
AS
	DECLARE @TrayID INT;
	DECLARE @Tray_Position INT;
	
	DECLARE @FailThreshold FLOAT;
	DECLARE @Number FLOAT;
	DECLARE @value BIT;

	DECLARE @PartsTested INT;
BEGIN
	-- Get all untested units which are in a full tray and owned by the requested lane
	DECLARE UnitCursor CURSOR
	FOR SELECT Position, TrayID FROM UnitInfo WHERE LaneID = @LaneID AND UnitTested = 0 AND PartsInTray = TrayCapacity;
	OPEN UnitCursor;

	-- Get the fail threshold (0-1) for the worker
	SELECT @FailThreshold = (1 - Fail_Rate / 100.0) FROM LaneWorkerInfo WHERE LaneID = @LaneID;

	FETCH NEXT FROM UnitCursor INTO @Tray_Position, @TrayID;

	SET @PartsTested = 0;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Generate a random decimal value (approximated to 5 significant digits)
		SELECT @Number = abs(checksum(rand()) % 10000.0) / 10000.0;
		
		-- If this part fails, set its result to 1, otherwise set it to 0
		IF @Number > @FailThreshold
			UPDATE Test_Unit SET Tested = 1, Test_Result = 1 WHERE Tray_Position = @Tray_Position AND Test_TrayID = @TrayID;
		ELSE
			UPDATE Test_Unit SET Tested = 1, Test_Result = 0 WHERE Tray_Position = @Tray_Position AND Test_TrayID = @TrayID;

		SET @PartsTested = @PartsTested + 1;

		FETCH NEXT FROM UnitCursor INTO @Tray_Position, @TrayID;
	END
	
	CLOSE UnitCursor;
	DEALLOCATE UnitCursor;

	SELECT @PartsTested as 'Parts Tested';

END
GO



--Creates a part from the given lane, will handle full tray and gives a new tray
Create or Alter Procedure Part_Created
	@LaneID int
As
	Declare @Position int
	Declare @TrayID int
	Declare @TrayCap int

	Declare @Harness_Count int
	Declare @Reflector_Count int
	Declare @Housing_Count int
	Declare @Lens_Count int
	Declare @Bulb_Count int
	Declare @Bezel_Count int

Begin
	
	Select @Harness_Count = Harness_Count, @Reflector_Count = Reflector_Count, @Housing_Count = Housing_Count, @Lens_Count = Lens_Count, @Bulb_Count = Bulb_Count, @Bezel_Count = Bezel_Count from lane
	where LaneID = @LaneID

	if(@Harness_Count > 0 and @Reflector_Count > 0 and @Housing_Count > 0 and @Lens_Count > 0 and @Bulb_Count > 0 and @Bezel_Count > 0)
	Begin
		UPDATE Lane SET Harness_Count = Harness_Count - 1, 
						Reflector_Count = Reflector_Count -1, 
						Housing_Count = Housing_Count -1,
						Lens_Count = Lens_Count -1,
						Bulb_Count = Bulb_Count -1,
						Bezel_Count = Bezel_Count -1 
					WHERE LaneID = @LaneID
	
		Select Top 1 @TrayID = Test_TrayID From Test_Tray Where LaneID = @LaneID Order by Test_TrayID desc
	
		Select @Position = Count(*) From Test_Unit Where Test_TrayID = @TrayID
		Select @TrayCap = Capacity From Test_Tray Where Test_TrayID = @TrayID

		Set @Position = @Position + 1

		If @Position > @TrayCap
		Begin
		
			Set @Position = 1

			Insert into Test_Tray (LaneID, Capacity)
				Select @LaneID, [value]
				From Config
				where [key] = 'Test_Tray_Cap'
				
			Select Top 1 @TrayID = Test_TrayID From Test_Tray Where LaneID = @LaneID Order by Test_TrayID desc

		End

		Update Test_Tray Set Part_Count = Part_Count + 1 Where Test_TrayID = @TrayID


		Insert into Test_Unit(Tray_Position, Test_TrayID, WorkerID)
			Select @Position, @TrayID, Lane.WorkerID
			From Lane
			Where LaneID = @LaneID

	End

End
go



--Check what bins need to be filled and add the tags to the event table
Create Trigger Check_Bins
on Lane
for Update
As
	Declare @Harness_Count int
	Declare @Reflector_Count int
	Declare @Housing_Count int
	Declare @Lens_Count int
	Declare @Bulb_Count int
	Declare @Bezel_Count int

	Declare @LaneID int

	Declare @WorkerID int
Begin

	Select @WorkerID = WorkerID from inserted

	Select @LaneID = LaneID, @Harness_Count = Harness_Count, @Reflector_Count = Reflector_Count, @Housing_Count = Housing_Count, @Lens_Count = Lens_Count, @Bulb_Count = Bulb_Count, @Bezel_Count = Bezel_Count from lane
	where WorkerID = @WorkerID

	if(@Harness_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Harness_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Harness_Count', @LaneID)
	end

	if(@Reflector_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Reflector_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Reflector_Count', @LaneID)
	end

	if(@Housing_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Housing_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Housing_Count', @LaneID)
	end

	if(@Lens_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Lens_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Lens_Count', @LaneID)
	end

	if(@Bulb_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Bulb_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Bulb_Count', @LaneID)
	end

	if(@Bezel_Count <= 5 and not exists (Select [key] from [event] where [key] = 'Bezel_Count' and [value] = @LaneID))
	begin
		insert into [Event] values ('Bezel_Count', @LaneID)
	end

End
go



--Fill the bins in the event table
create procedure Fill_Bins
As

declare @column varchar(25)
declare @lane int

Begin

	select top 1 @column = [key], @lane = CONVERT(INT, [Value]) from [event] order by [value]
	print @column

	while (@column != 'Last_Run')
		begin

		EXEC ('update Lane set ' + @column + ' += (select [Value] from config where [key] = ''' + @column + ''') where laneID = ' + @lane)

		Delete from [event] where [key] = @column and [value] = @lane

		select top 1 @column = [key], @lane = CONVERT(INT, [Value]) from [event] order by [value]

		print @column
	end

	Delete from [event] where [key] = @column and [value] = @lane

End
go



-- Gets the percentage finished of the current lane (range [0-1]).
-- If the result is >1, the item is finished and another has not been queued
CREATE PROCEDURE Get_Remaining_Time (
	@LaneID INT
)
AS
	DECLARE @TimeElapsed FLOAt
	DECLARE @TotalTime FLOAt
BEGIN
	SELECT @TimeElapsed = CONVERT(FLOAt, DATEDIFF(second, [Start_Time], GETDATE())),
		   @TotalTime = CONVERT(FLOAt, DATEDIFF(second, [Start_Time], [End_Time]))
		   FROM Lane WHERE LaneID = @LaneID
	
	SELECT (@TimeElapsed / @TotalTime) AS 'Percent Finished'
END
GO



-- Updates a lane's start and finish times to the current time and the current time + seconds to complete
CREATE PROCEDURE Update_Lane_Times (
	@LaneID INT,
	@SecondsToComplete FLOAT
)
AS
BEGIN
	UPDATE Lane SET
		Start_Time = GETDATE(),
		End_Time = DATEADD(second, @SecondsToComplete, GETDATE())
		WHERE LaneID = @LaneID
END
GO



-- Gets the time to the next runner check in percent (0-1)
CREATE PROCEDURE Get_Runner_Time
AS
	DECLARE @RunnerDelay FLOAT
	DECLARE @LastRun DATETIME
BEGIN
	
	-- The length of (real) time between runner runs
	SELECT @RunnerDelay = CONVERT(FLOAT, (SELECT [Value] FROM Config WHERE [Key] = 'Runner_Delay'))
						/ CONVERT(FLOAT, (SELECT [Value] FROM Config WHERE [Key] = 'Time_Scale'));

	-- Only one 'Last_Run' can be in the table at once
	SELECT @LastRun = CONVERT(DATETIME, [Value]) FROM [Event] WHERE [Key] = 'Last_Run';

	SELECT (CONVERT(FLOAT, DATEDIFF(second, @LastRun, GETDATE())) / @RunnerDelay) AS 'Percent To Check',
	@RunnerDelay, @LastRun;
END
GO


-- Updates the last runner time to 'right now'
CREATE PROCEDURE Update_Runner_Time
AS
BEGIN

	DELETE FROM [Event] WHERE [Key] = 'Last_Run';
	INSERT INTO [Event] ([Key], [Value]) VALUES ('Last_Run', GETDATE());
END
GO

-- Gets the good parts, bad parts, and percent yield for a given lane
CREATE PROCEDURE Get_Lane_Statistics (
	@LaneID INT
)
AS
	DECLARE @GoodCount INT
	DECLARE @BadCount INT
	DECLARE @PercentYield FLOAT
BEGIN
	
	SELECT @GoodCount = COUNT(*) FROM Test_Unit
		INNER JOIN Test_Tray
		ON Test_Unit.Test_TrayID = Test_Tray.Test_TrayID
		WHERE Test_Tray.LaneID = @LaneID AND Test_Unit.Tested = 1 AND Test_Unit.Test_Result = 0;

	SELECT @BadCount = COUNT(*) FROM Test_Unit
		INNER JOIN Test_Tray
		ON Test_Unit.Test_TrayID = Test_Tray.Test_TrayID
		WHERE Test_Tray.LaneID = @LaneID AND Test_Unit.Tested = 1 AND Test_Unit.Test_Result = 1;

	IF (@GoodCount + @BadCount) = 0
		SET @PercentYield = 0;
	ELSE
		SET @PercentYield = (@GoodCount / CONVERT(FLOAT, @GoodCount + @BadCount));
	
	SELECT @GoodCount AS 'Good Parts', @BadCount AS 'Bad Parts', @PercentYield as '% Yield';
END
GO
