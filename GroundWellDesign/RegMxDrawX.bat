@Echo off
Echo._______________________________________________________________
Echo.
Echo.   正在注册控件(如注册失败请以管理员身份运行)....
Echo.
Echo.________________________________________________________________

if /i "%PROCESSOR_IDENTIFIER:~0,3%" == "X86" goto X32

%~d0
cd %~dp0
Echo.注册32位控件
cd ReleaseFile
%systemroot%\syswow64\regsvr32 MxDrawX.ocx
exit

:X32
cd ReleaseFile
regsvr32 MxDrawX.ocx
exit