cd /d ../Bin
dotnet Tools.dll --AppType=AttrExporter

set "source=..\Config"
set "destination=..\..\ETPub\Server\Config"

xcopy "%source%\*" "%destination%\" /E /H /C /Y

echo 文件已成功复制！
pause