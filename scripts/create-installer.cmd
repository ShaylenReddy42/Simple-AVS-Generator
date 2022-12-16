@ECHO off

CD "%~dp0"

ECHO Checking for Inno Setup
ECHO.

SET ISCC=%ProgramFiles(x86)%\Inno Setup 6\iscc.exe

IF EXIST "%ISCC%" (
    ECHO Found Inno Setup
    ECHO.
) else (
    ECHO Inno Setup is not installed, exiting early
    ECHO.

    GOTO END
)

ECHO Creating installer

"%ISCC%" installer.iss

ECHO.
ECHO Installer was created at the installer directory at the root of the repository
ECHO.

:END

PAUSE
