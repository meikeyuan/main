if /i "%PROCESSOR_IDENTIFIER:~0,3%" == "X86" goto X32

%~d0
cd %~dp0
%systemroot%\syswow64\regsvr32 MxDrawX.ocx
exit

:X32
cd ReleaseFile
regsvr32 MxDrawX.ocx
exit