@ECHO off

IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" (
	@ECHO Found Visual Studio 2022
	@ECHO.
	@CALL "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"
	@ECHO.
	SET GENERATOR=Visual Studio 17
) ELSE IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat" ( 
	@ECHO Found Visual Studio 2019
	@ECHO.
	@CALL "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
	@ECHO.
	SET GENERATOR=Visual Studio 16
) ELSE (
	@ECHO Visual Studio 2022 nor 2019 could be found
	@GOTO END
)

@MD "%~dp0build"

cmake -G "%GENERATOR%" -A "x64" -S "%~dp0source" -B "%~dp0build"
@ECHO.

IF EXIST "%~dp0build\SimpleAVSGenerator.sln" ( 
	MSBuild /property:Configuration="Release" "%~dp0build\SimpleAVSGenerator.sln"
	@ECHO.
)

IF EXIST "%~dp0build\Release\Simple AVS Generator.exe" ( 
	@COPY /Y "%~dp0build\Release\Simple AVS Generator.exe" "%~dp0" /V
	@ECHO.
	@ECHO Project Built Successfully
	@ECHO.
)

:END

@PAUSE