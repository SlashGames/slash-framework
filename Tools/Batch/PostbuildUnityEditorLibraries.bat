REM 1. Create a new, empty project which only use is to copy all dlls to Unity.
REM    Make sure it uses .NET 3.5 to be compatible with Unity.
REM
REM 2. Reference all dll projects to this new project.
REM
REM 3. Put a copy of this batch file into the project folder (e.g. into a folder called Build).
REM 
REM 4. Put this line into the post-build event of your project 
REM    (replace PATH_TO_UNITY_PROJECT by the path to the unity project folder):
REM    "$(ProjectDir)Build\Postbuild.bat" PATH_TO_UNITY_PROJECT $(TargetDir)
REM
REM 5. The project will be compiled after all dll projects (as they are referenced by this project)
REM    and the dlls will be copied into the target directory of the project.
REM
REM    Then the post-build event will call this batch file, which will copy all dlls from the
REM    target directory into the plugins folder of your unity project except for unity dlls.

@echo OFF

REM Read arguments.
set PATH_TO_UNITY_PROJECT=%1
set DLL_TARGET_DIR=%PATH_TO_UNITY_PROJECT%Assets\Editor\Plugins\
set DLL_SOURCE_DIR=%2

REM Cleanup plugins folder.
echo Cleaning up target directory "%DLL_TARGET_DIR%"
del %DLL_TARGET_DIR%*.dll 

REM Don't copy Unity dlls, so hide them.
attrib +h UnityEngine.dll > NUL
attrib +h UnityEditor.dll > NUL
attrib +h nunit.framework.dll > NUL

REM Hide all dlls which are found in Unity main plugins folder (Unity gets confused if it founds a dll twice)
set UNITY_PLUGIN_DIR=%PATH_TO_UNITY_PROJECT%Assets\Plugins\
echo Searching in %UNITY_PLUGIN_DIR% for DLls.
for %%a in (%UNITY_PLUGIN_DIR%*.dll) do (
  echo Hiding dll %%~nxa
  attrib +h %%~nxa > NUL
)

REM Copy all dlls to Unity plugins folder.
echo Copying "%DLL_SOURCE_DIR%*.dll" to "%DLL_TARGET_DIR%"
xcopy "%DLL_SOURCE_DIR%*.dll" "%DLL_TARGET_DIR%" /d /y

REM Show hidden files again.
for %%a in (%UNITY_PLUGIN_DIR%*.dll) do (

  set FILENAME=%%~nxa
  echo Showing dll %%~nxa
  attrib -h %%~nxa > NUL
)

attrib -h UnityEngine.dll > NUL
attrib -h UnityEditor.dll > NUL
attrib -h nunit.framework.dll > NUL