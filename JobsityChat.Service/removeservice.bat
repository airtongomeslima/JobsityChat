set serviceName="JobsityChat.Service";

net start | %serviceName%  
if ERRORLEVEL 0 net stop %serviceName% (
	net stop %serviceName% 
	C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /LogToConsole=true /u JobsityChat.Service.exe
)