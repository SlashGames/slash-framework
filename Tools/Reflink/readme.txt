Those batch files can be used to reflink the framework to use them in a project.

This makes sense if the framework should be edited during development.

Usage:

1. Copy SetupFrameworkForProject.bat and SlashLibraries.txt files to your project
2. Adjust SlashLibraries.txt to list all desired libraries
3. Adjust SetupFrameworkForProject.bat: Set UNITY_PATH to point to your Unity project root (i.e. the folder with Assets, Libary, Temp,...)
4. Make sure you have defined the SLASH_FRAMEWORK environment variable to point to your Slash Framework root folder
5. Run the SetupFrameworkForProject.bat file
6. Adjust your solution files to include the Slash libraries in the build (Unity does this automatically)