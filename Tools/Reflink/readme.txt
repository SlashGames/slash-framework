Those batch files can be used to reflink the framework to use them in a project.

This makes sense if the framework should be edited during development.

Usage:

1. Copy batch files to project root (e.g. for Unity projects inside the Assets folder)
2. Check the SetupFramework.bat and comment all libraries that you don't need
3. Adjust your solution files to include the Slash libraries in the build (Unity does this automatically)