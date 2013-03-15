if %1 equ '' break

FOR /R %%f IN (*.*) DO (

	if "%%~xf"=="%1" (
		copy "%%f" "%%~pf\%%~nf"
	)
)
