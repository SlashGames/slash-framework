@echo off
@ if "%1" == "%9" goto help
@ if /i %1 EQU ? goto help
@ if /i %1 EQU help goto help
@ attrib +h +s %1
@ %2 %3 /Q
@ attrib -h -s %1
@ goto :EOF
:help
@echo        +-----------------------------------------------+
@echo        ¦ except filespec1 doscommand filespec2                 ¦
@echo        ¦                                                       ¦
@echo        ¦  filespec1  The files to exclude from doscommand      ¦
@echo        ¦  doscommmand The DOS command to execute on filespec2  ¦
@echo        ¦  filespec2  The files to execute doscommand against   ¦
@echo        ¦                                                       ¦
@echo        ¦ Example:                                              ¦
@echo        ¦                                                       ¦
@echo        ¦ except *.txt del *.*                                  ¦
@echo        ¦                                                       ¦
@echo        ¦Deletes all files except text files in the directory   ¦
@echo        +-----------------------------------------------+