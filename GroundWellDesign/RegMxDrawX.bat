Echo off
Echo._______________________________________________________________
Echo.   *********���澮��Ƴ���CAD��ͼģ��ؼ�ע��***********
Echo.             (��ע��ʧ�����Թ���Ա��������)
Echo.________________________________________________________________

if /i "%PROCESSOR_IDENTIFIER:~0,3%" == "X86" goto X32
%~d0
cd %~dp0
Echo.ע��32λ�ؼ�
cd ReleaseFile
%systemroot%\syswow64\regsvr32 MxDrawX.ocx
exit

:X32
cd ReleaseFile
regsvr32 MxDrawX.ocx
exit