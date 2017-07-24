Those batch files can be used to symlink the framework to use them in a project.

This makes sense if:
- The framework should be edited during development
- The framework is added as a submodule to a Unity project

Usage:

1. Copy SymlinkSlashFramework.bat and SlashLibraries.txt files from the Project folder to your project
2. Adjust SlashLibraries.txt to list all desired libraries
3. Adjust SymlinkSlashFramework.bat: Set SYMLINK_PATH to point to the folder that should contain the symlinks (e.g. Assets/SlashFramework for a Unity project)
4. Run the SymlinkSlashFramework.bat file
5. Adjust your solution files to include the Slash libraries in the build (Unity does this automatically)