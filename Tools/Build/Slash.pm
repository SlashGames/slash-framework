#!/usr/bin/perl

package Slash;
use Exporter;

@ISA = ('Exporter');
@EXPORT = ('pdb2Mdb');

use Cwd 'abs_path';

sub pdb2Mdb {
    if ($ENV{'MONO_PATH'}) {
        
        my $MONO_PATH = $ENV{'MONO_PATH'};
        
        # retrieve the argument
        my ($abs_path) = shift (@_);
        
        my $pdb2mdb_path = $MONO_PATH . "pdb2mdb.exe";
        my $pdb2mdb_call = '"' . $pdb2mdb_path . '" "' . $abs_path . '"';
        print $pdb2mdb_call;
        system($pdb2mdb_call);
        
    } else {
        print "Unity path not set in MONO_PATH environment variable, so pdbs won't be converted to mdbs.";
    }
}

1;