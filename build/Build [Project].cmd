@ECHO off

IF EXIST "%VS150COMCOMNTOOLS%VsDevCmd.bat" ( 
	@CALL "%VS150COMCOMNTOOLS%VsDevCmd.bat"
) ELSE (
	@ECHO MSBuild cannot be found, terminating
	@GOTO END
)

IF NOT EXIST "%~dp0solution\SimpleAVSGenerator.sln" (
	@CALL "%~dp0Build [Solution].cmd"
	
	IF EXIST "%~dp0solution\SimpleAVSGenerator.sln" ( 
		MSBuild /property:Configuration=Release "%~dp0solution\SimpleAVSGenerator.sln"
	)
) ELSE (
	MSBuild /property:Configuration=Release "%~dp0solution\SimpleAVSGenerator.sln"
)

IF EXIST "%~dp0solution\Release\Simple AVS Generator.exe" ( 
	@COPY /Y "%~dp0solution\Release\Simple AVS Generator.exe" "%~dp0" /V
	@ECHO Project Built Successfully
)

:END

@PAUSE