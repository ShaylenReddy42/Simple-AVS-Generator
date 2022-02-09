@ECHO off

IF NOT EXIST "%~dp0version.txt" (
	cmake -G "Visual Studio 17" -S . -B "%~dp0build"
	@ECHO.
)

@CALL "%~dp0dotnet-publish.cmd"
@ECHO.

@RD "%~dp0build" /S /Q
@RD "%~dp0Simple AVS Generator\bin" "%~dp0Simple AVS Generator\obj" /S /Q
@RD "%~dp0Simple AVS Generator.Core\bin" "%~dp0Simple AVS Generator.Core\obj" /S /Q
@DEL "%~dp0dotnet-publish.cmd"

:END

IF NOT EXIST "%~dp0version.txt" (
	@PAUSE
)