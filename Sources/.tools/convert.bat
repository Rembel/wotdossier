setlocal ENABLEDELAYEDEXPANSION
for /r .\ %%f IN (*.mo) do (
..\..\..\Tools\msgunfmt.exe %%f > %%~nf.po
..\..\..\Tools\resgenEx.exe %%~nf.po %%~nf.resx
)