@echo off

@md "%~dp0solution"

@cd "%~dp0solution"

cmake -G "Visual Studio 15 Win64" ../../source