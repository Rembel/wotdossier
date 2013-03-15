set FRAMEWORK_PATH=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set LOG_FILE=build.log

%FRAMEWORK_PATH%\MSBuild.exe prepareenv.proj /l:FileLogger,Microsoft.Build.Engine;logfile=%LOG_FILE%

if  %errorlevel% neq 0 goto error
exit /B 0

:error
echo ERROR!!!
pause
exit /B 1
