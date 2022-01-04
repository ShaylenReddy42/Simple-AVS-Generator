@ECHO off

IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" (
	@ECHO Found Visual Studio 2022
	@ECHO.
	@CALL "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"
	@ECHO.
	SET GENERATOR=Visual Studio 17
) ELSE (
	@ECHO Visual Studio 2022 could not be found
	@GOTO END
)

@MD "%~dp0build"

cmake -G "%GENERATOR%" -A "x64" -S . -B "%~dp0build"
@ECHO.

@CALL dotnet-publish.cmd
@ECHO.

@RD "%~dp0bin" "%~dp0build" "%~dp0obj" /S /Q
@DEL "%~dp0dotnet-publish.cmd"

:END

@PAUSE