@ECHO off

@MD "%~dp0solution"

@CD "%~dp0solution"

cmake -G "Visual Studio 15 Win64" ../../source