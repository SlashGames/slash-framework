#!/usr/bin/perl

use Cwd qw(abs_path);
use File::Basename;
use File::Spec qw(catfile);
use File::Copy::Recursive qw(dircopy);
use File::Copy qw(move);

# Wizards of the Slash framework:
# - setup project-name
#   Sets up a new project using the Slash framework using "project-name" as the project name.

my $script_dir = dirname(abs_path($0));
    
# Check system setup.
if (not $ENV{'SLASH_FRAMEWORK_HOME'}) {
    print "\nWARNING: Environment variable SLASH_FRAMEWORK_HOME not set, please make sure to set it so the framework projects can be found.";
}

if (not $ENV{'UNITY_PATH'}) {
    print "\nWARNING: Environment variable UNITY_PATH not set, please make sure to set it so the Unity tools can be found e.g. for converting pdbs to mdbs.";
}
    
my $mode = $ARGV[0];
if ($mode == "setup")
{
    print "Setting up new project.";
    
    # Get project name.
    my $project_name = $ARGV[1];
    while (length $project_name == 0)
    {
        print "\nPlease type in project name: ";
        $project_name = <STDIN>;
        chop($project_name);
    }
    
    # Create main directory.
    print "\nCreating new project '$project_name'.";
    unless(mkdir $project_name)
    {
        die "\nUnable to create directory $project_name";
    }
    
    # Copy project template to directory.
    my $template_dir = File::Spec->catfile($script_dir, "Template");
    print "\nCopying project structure from '$template_dir' to project directory.";
    dircopy($template_dir, $project_name);
    
    # Rename solution file.
    my $original_solution_file = File::Spec->catfile($project_name, "Source/project-name.sln");
    my $target_solution_file = File::Spec->catfile($project_name, "Source/$project_name.sln");
    print "\nRenaming solution file from '$original_solution_file' to '$target_solution_file'.";
    move $original_solution_file, $target_solution_file;
}