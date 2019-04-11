
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
	[Value] int
)

insert into Config 
	([key], [value])
Values
	('Time_Scale', 1),
	('Harness_Count', 55),
	('Reflector_Count', 35),
	('Housing_Count', 24),
	('Lens_Count', 40),
	('Bulb_Count', 60),
	('Bezel_Count', 75),
	('Test_Tray_Cap', 60);



Create Table [Event](
	[Key] varchar(30),
	[Value] int
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
	Is_Running int default 0
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
Values (0, 0.5);

Insert into Experience (Time_Deviancy, Fail_rate)
Values (.85, 0.15);

go



--Procedure to create a worker with the given experience
create Procedure Create_Worker
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
go
Create Procedure Create_Lane
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
	Select Lane.WorkerID, Experience.Time_Deviancy, Experience.Fail_Rate from Lane
	Inner Join Worker on Lane.WorkerID = Worker.WorkerID 
	Inner Join Experience on Worker.Experience_Level = Experience.Experience_Level
	Where LaneID = @LaneID
End
Go

--test the product
create Procedure Test_Product
	@Fail int,
	@workerID int
As
	Declare @Tray_pos int
	Declare @TrayID int
Begin
SELECT TOP 1 [Tray_Position]
			,[Test_TrayID]
			,[WorkerID]
			,[Tested]
			,[Test_Result]
		FROM [kanban].[dbo].[Test_Unit]
		Where WorkerID = @workerID
		order by Test_TrayID desc, Tray_Position desc

	Update Test_Unit Set Tested = 1, Test_Result = @Fail Where Test_TrayID = @TrayID and Tray_Position = @Tray_pos
	
End
Go



--Creates a part from the given lane, will handle full tray and gives a new tray
Create Procedure Part_Created
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
on Test_Unit
for Insert
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

	select top 1 @column = [key], @lane = [Value] from [event] order by [value]
	print @column

	while (@column != 'Last_Run')
		begin

		EXEC ('update Lane set ' + @column + ' += (select [Value] from config where [key] = ''' + @column + ''') where laneID = ' + @lane)

		Delete from [event] where [key] = @column and [value] = @lane

		select top 1 @column = [key], @lane = [Value] from [event] order by [value]

		print @column
	end

	Delete from [event] where [key] = @column and [value] = @lane

End
go