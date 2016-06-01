@echo off

set BATCH_DIR=%~dp0

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

set UNITY_PROJECT_DIR=%~2

cd %UNITY_PROJECT_DIR%/Assets
mkdir Slash.Framework

cd Slash.Framework
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Application
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.ECS
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.ECS.Blueprints
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.ECS.Processes
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Collections
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Reflection
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.System
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Math
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Unity.Common
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Serialization
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Diagnostics
call "%BATCH_DIR%/LinkFrameworkLibrary.bat" "%SLASH_FRAMEWORK%" Slash.Unity.DataBind.Ext

cd %BATCH_DIR%
