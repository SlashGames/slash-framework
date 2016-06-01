@echo off

if not [%1]==[] set SLASH_FRAMEWORK=%~1

if ["%SLASH_FRAMEWORK%"]==[""] (
  echo SLASH_FRAMEWORK variable not set.
  exit
)

rmdir %2
mklink /J %2 "%SLASH_FRAMEWORK%\Source\%2\Source"