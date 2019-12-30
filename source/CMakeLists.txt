cmake_minimum_required(VERSION 3.8)

include(CSharpUtilities)
include(FindGit)

project(
	SimpleAVSGenerator
	LANGUAGES CSharp
)

# Extracting version

set(TAG "0.0")
set(TAG_DISTANCE "0")
set(COMMIT_ID "commit")

if(${GIT_FOUND} AND EXISTS "${CMAKE_SOURCE_DIR}/../.git")
	execute_process(
		COMMAND ${GIT_EXECUTABLE} rev-list --all --abbrev-commit --abbrev=7 --max-count=1
		WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
		OUTPUT_VARIABLE COMMIT_ID
		OUTPUT_STRIP_TRAILING_WHITESPACE
		ERROR_QUIET
	)
	
	execute_process(
		COMMAND ${GIT_EXECUTABLE} rev-list ${COMMIT_ID} --count
		WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
		OUTPUT_VARIABLE TAG_DISTANCE
		OUTPUT_STRIP_TRAILING_WHITESPACE
		ERROR_QUIET
	)
	
	execute_process(
		COMMAND ${GIT_EXECUTABLE} describe --tags ${GIT_COMMIT_ID}
		WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
		OUTPUT_VARIABLE GIT_TAG
		OUTPUT_STRIP_TRAILING_WHITESPACE
	)
	
	if(GIT_TAG STREQUAL "")
		set(TAG "0.0")
	endif()
endif()

set(FILE_VERSION "${TAG}.0.${TAG_DISTANCE}")
set(INFORMATIONAL_VERSION "${TAG}+${TAG_DISTANCE} [${COMMIT_ID}]")

message(STATUS "Version: ${INFORMATIONAL_VERSION}")

configure_file("${CMAKE_SOURCE_DIR}/AssemblyInfo.cs.in" "${CMAKE_SOURCE_DIR}/Properties/AssemblyInfo.cs")

add_executable(
	${PROJECT_NAME} WIN32
	app.config
	Program.cs
	"Simple AVS Generator.cs"
	"Simple AVS Generator.Designer.cs"
	"Simple AVS Generator.resx"
	Properties/AssemblyInfo.cs
	Properties/Resources.resx
	Properties/Resources.Designer.cs
	Properties/Settings.cs
	Properties/Settings.settings
	Properties/Settings.Designer.cs
)

csharp_set_windows_forms_properties(
	"Simple AVS Generator.cs"
	"Simple AVS Generator.Designer.cs"
	"Simple AVS Generator.resx"
)

csharp_set_designer_cs_properties(
	Properties/Resources.resx
	Properties/Resources.Designer.cs
	Properties/Settings.cs
	Properties/Settings.settings
	Properties/Settings.Designer.cs
)

set_target_properties(
	${PROJECT_NAME} 
	PROPERTIES
		VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
		VS_DOTNET_REFERENCES "System;System.Windows.Forms;System.Data;System.Drawing"
		OUTPUT_NAME "Simple AVS Generator"
)