@echo off 
REM turn off command output
REM Billy Parmenter
REM January/3/2018
REM ProcessText.bat
REM open given text file with chosen editor or print


REM get the users input
SET /P file=Enter the pathname of text file to process: 

REM check if the user input anything
IF "%file%"=="" (
	REM if not goto the null error location
	goto nullError 
)

REM check if the given file exists
IF EXIST %file% ( 
	REM if the file exists clear the screen and output the menu
	cls 
	echo 1. Open in Notepad
	echo 2. Open in Word
	echo 3. Open in Notepad++
	echo 4. Print
	REM if the file does not exist
) ELSE ( 
	REM go to that error catch
	goto doesNotExist  
)

REM as long as the file exists then run the .exe file to collect user input and return it as an errorlevel
myChoice.exe

REM set the errorlevel to the returnValue
set returnValue=%errorlevel%

REM if the user didnt input a number
IF "%returnValue%"=="-1" (
	REM go to that error catch
	goto notANumber 
	REM else go to the users chosen function
) ELSE IF "%returnValue%"=="1" (

	C:\WINDOWS\system32\notepad.exe "%file%"
	
) ELSE IF "%returnValue%"=="2" (

	"C:\Program Files\Microsoft Office\Office16\WINWORD.EXE" "%file%"

) ELSE IF "%returnValue%"=="3" (

	"C:\Program Files (x86)\Notepad++\notepad++.exe" "%file%"
	
) ELSE IF "%returnValue%"=="4" (

	C:\WINDOWS\system32\notepad.exe /p "%file%"
REM if the user input a number outside of the range
) ELSE ( 
	REM tell them that they input something wrong
	echo Number out of range 
	REM go to the end of the program
	goto end 
)
REM go to the end of the program
goto end 

:doesNotExist
REM tell them that they input something wrong
echo File does not exist... 
REM go to the end of the program
goto end 

:notANumber
REM tell them that they input something wrong
echo Input is not a number or number is too large... 
REM go to the end of the program
goto end 

:nullError
REM tell them that they input something wrong
echo No pathname input... 
REM go to the end of the program
goto end 

:end
REM give user time to read output
pause 