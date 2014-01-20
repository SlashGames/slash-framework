# 1. Create a new, empty project which only use is to copy all dlls to Unity.
#    Make sure it uses .NET 3.5 to be compatible with Unity.
#
# 2. Reference all dll projects to this new project.
#
# 3. Put a copy of this script into the project folder (e.g. into a folder called Build).
# 
# 4. Put this line into the post-build event of your project 
#    (replace PATH_TO_UNITY_PROJECT by the relative path to the unity project folder):
#    perl "$(ProjectDir)Build\Postbuild.pl" PATH_TO_UNITY_PROJECT $(TargetDir)
# 
#    Mono:
#    perl "${ProjectDir}Build\Postbuild.pl" PATH_TO_UNITY_PROJECT ${TargetDir}
#
# 5. The project will be compiled after all dll projects (as they are referenced by this project)
#    and the dlls will be copied into the target directory of the project.
#
# 6. Then the post-build event will call this script, which will copy all dlls from the
#    target directory into the plugins folder of your unity project except for unity dlls.
#
# 7. Additionally all pdb files are converted to mdb files, so Unity has debug information for the dlls.
#    This allows one to debug the dlls with MonoDevelop and prints out the line numbers in log messages.
#

use strict;
use warnings;
use File::Copy;
use File::Basename;
use File::Path qw(make_path);
use File::Path qw(remove_tree);
use Cwd 'abs_path';
use Cwd 'cwd';

# Read arguments.
my $COMMAND = $ARGV[0];
my $PATH_TO_UNITY_PROJECT = abs_path($ARGV[1]);
my $DLL_GAME_TARGET_DIR = "${PATH_TO_UNITY_PROJECT}/Assets/Plugins/Game";
my $DLL_TARGET_DIR = "${PATH_TO_UNITY_PROJECT}/Assets/Editor/Plugins/Game";
my $DLL_SOURCE_DIR = abs_path($ARGV[2]);
my $AOT_COMPATLYZER_PATH = abs_path(dirname($0)."/AOT-Compatlyzer/AOTCompatlyzer.exe");

print "Paths:\n${DLL_SOURCE_DIR}\n${DLL_TARGET_DIR}\n";

# Collect dlls to process
my @dlls = ();
for my $file (<"${DLL_SOURCE_DIR}/*.dll">) 
{
    my $basename = basename($file);
    
    # Don't copy Unity dlls.
    if ($basename eq "UnityEngine.dll" || $basename eq "UnityEditor.dll") {
        next;
    }
    
    # Don't process game dlls.
    my $process_dll = 1;
    for my $game_dll (<"${DLL_GAME_TARGET_DIR}/*.dll">)
    {
        my $dll_basename = basename($game_dll);     
        if ($basename eq $dll_basename) {
            $process_dll = 0;
            last;
        }
    }
    
    if ($process_dll) {
        print "Processing DLL: " . $file . "\n";
        push(@dlls, $file);
    }
}

if ($COMMAND eq "run") {

    if ($ENV{'UNITY_PATH'}) {
        
        my $UNITY_PATH = $ENV{'UNITY_PATH'};

        # Convert pdb to mdb and copy to plugins folder.
        print "Convert pdb to mdb in '${DLL_SOURCE_DIR}/*.dll'\n";
        my $pwd = cwd();
        chdir(${DLL_SOURCE_DIR});
        foreach my $file (@dlls) 
        {
            print "Converting '${file}' to mdb \n";
            
            my $abs_path = abs_path($file);
            my $pdb2mdb_call = '"' . $UNITY_PATH . 'Data/MonoBleedingEdge/lib/mono/4.0/pdb2mdb.exe" "' . $abs_path . '"';
            system($pdb2mdb_call);

            print "Postprocessing DLL \"${abs_path}\" to be AOT compatible\n";
            
            my $aotcompatlyzer_call = '"' . $AOT_COMPATLYZER_PATH . '" "' . $abs_path . '"';
            system($aotcompatlyzer_call);
        }
        chdir($pwd);
    } else {
        print "Unity path not set in UNITY_PATH environment variable, so pdbs won't be converted to mdbs.";
    }

    # Make sure target folder exists.
    unless(-e $DLL_TARGET_DIR or make_path $DLL_TARGET_DIR) {
        die "Unable to create $DLL_TARGET_DIR: $!";
    }

    # Copy all dlls to Unity plugins folder.
    print "Copying '${DLL_SOURCE_DIR}/*.dll' and '${DLL_SOURCE_DIR}/*.mdb' to '${DLL_TARGET_DIR}'\n";

    foreach my $file (@dlls) 
    {
        my $basename = basename($file);
        
        # Don't copy Unity dlls.
        if ($basename eq "UnityEngine.dll" || $basename eq "UnityEditor.dll") {
            next;
        }
        
        # Don't copy game dlls.
        my $copy_dll = 1;
        for my $game_dll (<"${DLL_GAME_TARGET_DIR}/*.dll">)
        {
            my $dll_basename = basename($game_dll);     
            if ($basename eq $dll_basename) {
                $copy_dll = 0;
                last;
            }
        }
        
        if ($copy_dll) {
            print "Copying ${basename} to ${DLL_TARGET_DIR}/${basename}\n";
            copy($file, "${DLL_TARGET_DIR}/${basename}") or die "copy $file failed: $!";
        }
    }

    foreach my $file (@dlls) 
    {
        my $mdbFile = $file . ".mdb";
        my $basename = basename($mdbFile);
        
        if (-e $mdbFile) {
            print "Copying ${basename} to ${DLL_TARGET_DIR}/${basename}\n";
            copy($mdbFile, "${DLL_TARGET_DIR}/${basename}") or die "copy $mdbFile failed: $!";
        }
    }
}

if ($COMMAND eq "clean") {
    
    # Remove dll folder.
    if (-e $DLL_TARGET_DIR) {
        remove_tree $DLL_TARGET_DIR;
    }
    
}
