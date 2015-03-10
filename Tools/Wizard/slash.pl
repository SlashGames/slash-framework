#!/usr/bin/perl

use Cwd qw(abs_path);
use File::Basename;
use File::Spec qw(catfile);
use File::Copy::Recursive qw(dircopy);
use File::Copy qw(move);
use File::Find;

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
    
    # Create main directory if it doesn't exist.
    unless(-d $project_name) 
    {
        print "\nCreating new project '$project_name'.";
        unless(mkdir $project_name)
        {
            die "\nUnable to create directory $project_name";
        }
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
    
    # Rename game folder
    my $original_game_folder = File::Spec->catfile($project_name, "Source/Unity/Assets/Game");
    my $target_game_folder = File::Spec->catfile($project_name, "Source/Unity/Assets/$project_name.Unity");
    print "\nRenaming game folder from '$original_game_folder' to '$target_game_folder'.";
    move $original_game_folder, $target_game_folder;
    
    # Rename unity folder
    my $original_unity_folder = File::Spec->catfile($project_name, "Source/Unity");
    my $target_unity_folder = File::Spec->catfile($project_name, "Source/$project_name.Unity");
    print "\nRenaming Unity folder from '$original_unity_folder' to '$target_unity_folder'.";
    move $original_unity_folder, $target_unity_folder;
    
    # Replace namespace in source files.
    print "\nReplacing namespaces in source files.";
    my @files;
    sub find_cs {
        
        my $F = $File::Find::name;

        if ($F =~ /cs$/ ) {        
            push @files, $F;
            print "$F\n";
        }
    }

    find({ wanted => \&find_cs }, $project_name);
    foreach my $filename (@files)
    {
        print "\nReplacing namespaces in '$filename'.";    
    
        @ARGV = ($filename);
        $^I = '.bak';
        while (<>)
        {
            $_ =~ s/{GAME}/$project_name/;
            print $_ . "\n";
        }
        
        # Delete backup file.
        unlink $filename . $^I;
    }
}