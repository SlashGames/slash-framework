@echo off

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

set SOURCE_FOLDER=%SLASH_FRAMEWORK%\Source\%~2\Source
if [%3] == [] (
  set TARGET_FOLDER=%~2 ) else (
  set TARGET_FOLDER=%~3
)

echo Link %SOURCE_FOLDER% to %TARGET_FOLDER%

rmdir "%TARGET_FOLDER%"
mklink /J "%TARGET_FOLDER%" "%SOURCE_FOLDER%"