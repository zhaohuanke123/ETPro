dotnet publish -r linux-x64 --no-self-contained --no-dependencies -c Release

set "source=.\Bin\linux-x64\publish"
set "destination=..\ETPub\Server\publish"

xcopy "%source%\*" "%destination%\" /E /H /C /Y

pause