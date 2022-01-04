@ECHO off

@MD "%~dp0build"

IF NOT EXIST "%~dp0version.txt" (
	cmake -G "Visual Studio 17" -S . -B "%~dp0build"
	@ECHO.
)

@CALL "%~dp0dotnet-publish.cmd"
@ECHO.

@RD "%~dp0bin" "%~dp0build" "%~dp0obj" /S /Q
@DEL "%~dp0dotnet-publish.cmd"

:END

IF NOT EXIST "%~dp0version.txt" (
	@PAUSE
)