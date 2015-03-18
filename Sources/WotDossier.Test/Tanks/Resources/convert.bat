setlocal ENABLEDELAYEDEXPANSION
for /r .\ %%f IN (*.mo) do (
..\..\..\..\Automation\Localize\msgunfmt.exe %%f > %%~nf.po
..\..\..\..\Automation\Localize\resgenEx.exe %%~nf.po %%~nf.resx
)