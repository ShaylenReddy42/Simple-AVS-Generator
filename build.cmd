@ECHO off

@CD "%~dp0"

@ECHO Run CMake
@ECHO.
cmake -G "Visual Studio 17" -S . -B build

@ECHO.
@ECHO Restore Solution
@ECHO.
dotnet restore

@ECHO.
@ECHO Build Solution
@ECHO.
dotnet build -c Debug

@ECHO.
@ECHO Clean Older Tests If They Exist
@ECHO.
IF EXIST SimpleAVSGeneratorCore.Tests\TestResults (
	@RD SimpleAVSGeneratorCore.Tests\TestResults /S /Q
)

@ECHO Run Unit Tests 
@ECHO.
dotnet test -c Debug --collect:"XPlat Code Coverage"

@ECHO.
@ECHO Restore Local Tools
@ECHO.
dotnet tool restore

@ECHO.
@ECHO Generate HTML Code Coverage Report
@ECHO.
dotnet tool run reportgenerator -reports:"SimpleAVSGeneratorCore.Tests\TestResults\*\*.xml" -reporttypes:HtmlInline_AzurePipelines -targetDir:"CodeCoverage"

@ECHO.
@ECHO Publish the Final Executable
@ECHO.
@CALL dotnet-publish.cmd

@ECHO.
@ECHO Cleanup
@ECHO.
@RD build /S /Q
@DEL dotnet-publish.cmd

@PAUSE