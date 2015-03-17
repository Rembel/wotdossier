setlocal ENABLEDELAYEDEXPANSION
for /r .\ %%f IN (*.mo) do (
msgunfmt.exe %%f > %%f.po
)