@echo off

cls

call setenv.cmd

set PATH=%PATH%;%CS_HOME%
set CS="%CS_HOME%\cs.exe"

%CS% /v %CS_PROJECT%

goto finish

:csp_not_found

echo %CS_PROJECT% properties file not found.

:finish

pause
