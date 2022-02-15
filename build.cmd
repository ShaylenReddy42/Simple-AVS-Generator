@ECHO off

IF NOT EXIST "%~dp0version.txt" (
	cmake -G "Visual Studio 17" -S . -B "%~dp0build"
	@ECHO.
)

@CALL "%~dp0dotnet-publish.cmd"
@ECHO.

@RD "%~dp0build" /S /Q
@RD "%~dp0SimpleAVSGenerator\bin" "%~dp0SimpleAVSGenerator\obj" /S /Q
@RD "%~dp0SimpleAVSGenerator.Core\bin" "%~dp0SimpleAVSGenerator.Core\obj" /S /Q
@RD "%~dp0SimpleAVSGenerator.Core.Tests\bin" "%~dp0SimpleAVSGenerator.Core.Tests\obj" /S /Q
@DEL "%~dp0dotnet-publish.cmd"

:END

IF NOT EXIST "%~dp0version.txt" (
	@PAUSE
)