cd /d ../Bin
dotnet Tools.dll --AppType=ExporterAll

set "source=..\Config"
set "destination=..\..\ETPub\Server\Config"

xcopy "%source%\*" "%destination%\" /E /H /C /Y

echo �ļ��ѳɹ����ƣ�
pause