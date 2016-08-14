#!/usr/bin/perl

use strict;
use warnings;
use File::Copy;
use File::Basename;
use File::Find;
use File::Path qw(make_path);
use File::Path qw(remove_tree);
use Cwd 'abs_path';
use Cwd 'cwd';
use Slash;

# Read arguments.
my $DLL_SOURCE_DIR = abs_path("$ARGV[0]");
my $TARGET_FOLDER = "$ARGV[1]";

# Delete target folder.
if (-e $TARGET_FOLDER)
{
    #remove_tree($TARGET_FOLDER, {keep_root => 1, verbose => 1, error => \my $err}) or die "Can't clean target folder $TARGET_FOLDER";
}

# Make sure target folder exists.
unless(-e $TARGET_FOLDER or make_path $TARGET_FOLDER) {
    die "Unable to create $TARGET_FOLDER: $!";
}

my $DLL_TARGET_DIR = abs_path($TARGET_FOLDER);

# Copy all dlls to target folder.
print "Copying '${DLL_SOURCE_DIR}/*.dll' and '${DLL_SOURCE_DIR}/*.pdb' to '${DLL_TARGET_DIR}'\n";

for my $file (<"${DLL_SOURCE_DIR}/*.dll">) 
{
    my $basename = basename($file);
        
    print "Copying ${basename} to ${DLL_TARGET_DIR}/${basename}\n";
    copy($file, "${DLL_TARGET_DIR}/${basename}") or die "copy $file failed: $!";
}

for my $file (<"${DLL_SOURCE_DIR}/*.pdb">) 
{
    my $basename = basename($file);
        
    print "Copying ${basename} to ${DLL_TARGET_DIR}/${basename}\n";
    copy($file, "${DLL_TARGET_DIR}/${basename}") or die "copy $file failed: $!";
}

print "Paths:\n${DLL_SOURCE_DIR}\n${DLL_TARGET_DIR}\n\n";

# Convert pdb to mdb and copy to plugins folder.
print "Convert pdb to mdb in '${DLL_TARGET_DIR}/*.dll'\n";

# Set mono path.
# TODO(co): Use Unity tool again if a working one is provided with Unity 5.
$ENV{'MONO_PATH'} = File::Basename::dirname(abs_path($0)) . "/mono/4.0/";

my $pwd = cwd();
chdir(${DLL_TARGET_DIR});
for my $file (<"${DLL_TARGET_DIR}/*.dll">) 
{
    print "Converting '${file}' to mdb \n";
    
    my $abs_path = abs_path($file);
    pdb2Mdb($abs_path);
}
chdir($pwd);

# Delete pdbs and special dlls
sub find_file_to_delete {
    my $F = $File::Find::name;

    if ($F =~ /UnityEngine.dll$/) {
        print "$F\n";
        unlink $F;
    }
	
    if ($F =~ /UnityEditor.dll$/) {
        print "$F\n";
        unlink $F;
    }
	
    if ($F =~ /UnityEngine.UI.dll$/) {
        print "$F\n";
        unlink $F;
    }
	
    if ($F =~ /.pdb$/) {
        print "$F\n";
        unlink $F;
    }
}

if (-e $DLL_TARGET_DIR) {
    print "Cleaning target dir '${DLL_TARGET_DIR}':\n\n";
    find({ wanted => \&find_file_to_delete, no_chdir=>1}, $DLL_TARGET_DIR);
}   