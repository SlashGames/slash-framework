:MakeRelative file base -- makes a file name relative to a base path
::                      -- file [in,out] - variable with file name to be converted, or file name itself for result in stdout
::                      -- base [in,opt] - base path, leave blank for current directory
:$created 20060101 :$changed 20080219 :$categories Path
:$source http://www.dostips.com
SETLOCAL ENABLEDELAYEDEXPANSION
set src=%~1
if defined %1 set src=!%~1!
set bas=%~2
if not defined bas set bas=%cd%
for /f "tokens=*" %%a in ("%src%") do set src=%%~fa
for /f "tokens=*" %%a in ("%bas%") do set bas=%%~fa
set mat=&rem variable to store matching part of the name
set upp=&rem variable to reference a parent
for /f "tokens=*" %%a in ('echo.%bas:\=^&echo.%') do (
    set sub=!sub!%%a\
    call set tmp=%%src:!sub!=%%
    if "!tmp!" NEQ "!src!" (set mat=!sub!)ELSE (set upp=!upp!..\)
)
set src=%upp%!src:%mat%=!
( ENDLOCAL & REM RETURN VALUES
    IF defined %1 (SET %~1=%src%) ELSE ECHO.%src%
)
EXIT /b