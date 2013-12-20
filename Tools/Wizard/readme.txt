Required Perl packages:
- File-Copy-Recursive

Installation
------------

Add environment variables to use generated projects:

SLASH_FRAMEWORK_HOME    Path to the Slash Games Framework (e.g. C:\Projects\C#\Framework)
UNITY_PATH              Path to Unity (e.g. C:\Program Files (x86)\Unity\Editor\)

You can also add the wizard directory to your "Path" environment variable, so you don't have to provide the whole path to the wizard at the command line.

Setup project
-------------

Execute in location where project should be created. Will copy project template to target location and renames files accordingly.

Usage:
slash.pl setup project-name

- project-name: Name of the project, will be used as the name of the resultant folder and solution file.