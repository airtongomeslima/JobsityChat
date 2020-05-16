set serviceAddress="C:\JobsityChat\Service\"

if not exist "%serviceAddress%" mkdir %serviceAddress%

set mypath=%cd%
@echo %mypath%

%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild JobsityChat.Service.csproj /p:Configuration=Release /p:Platform="Any CPU"

xcopy "%mypath%/bin/Release" "%serviceAddress%" /K /D /H /Y
