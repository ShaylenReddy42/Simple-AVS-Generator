cmake_minimum_required(VERSION 3.8)

include(CSharpUtilities)
include(FindGit)

project(
	SimpleAVSGenerator
	LANGUAGES CSharp
)

# Extracting version

set(TAG "0.0")
set(REVISION "0")
set(COMMIT_ID "unknown")

if(${GIT_FOUND} AND EXISTS "${CMAKE_SOURCE_DIR}/../.git")
	execute_process(
		COMMAND ${GIT_EXECUTABLE} rev-list --all --abbrev-commit --abbrev=7 --max-count=1
		WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
		OUTPUT_VARIABLE COMMIT_ID
		OUTPUT_STRIP_TRAILING_WHITESPACE
		ERROR_QUIET
	)
	
	execute_process(
		COMMAND ${GIT_EXECUTABLE} describe --tags --abbrev=0
		WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
		OUTPUT_VARIABLE TAG
		OUTPUT_STRIP_TRAILING_WHITESPACE
		ERROR_QUIET
	)

	if(TAG STREQUAL "")
		set(TAG "0.0")
		
		execute_process(
			COMMAND ${GIT_EXECUTABLE} rev-list ${COMMIT_ID} --count
			WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
			OUTPUT_VARIABLE REVISION
			OUTPUT_STRIP_TRAILING_WHITESPACE
			ERROR_QUIET
		)
	else()
		execute_process(
			COMMAND ${GIT_EXECUTABLE} describe --tags
			WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
			OUTPUT_VARIABLE FULL_TAG
			OUTPUT_STRIP_TRAILING_WHITESPACE
			ERROR_QUIET
		)
		
		if(FULL_TAG STREQUAL TAG)
			set(REVISION 0)
		else()
			string(REGEX MATCH "-[0-9]+-" REVISION "${FULL_TAG}")
			string(REGEX REPLACE "-" "" REVISION "${REVISION}")
		endif()
	endif()
endif()

# CMake's string length function stores length in bytes not number
# of characters so no point in preparing for tags like 1.0.1
# Sticking to tags like 1.0, 1.1 for now [if I do tag :D]

# string(LENGTH TAG TAG_LENGTH)

# message(STATUS "Tag: ${TAG} Length: ${TAG_LENGTH}")

# if(TAG_LENGTH STREQUAL "3")
# 	set(FILE_VERSION "${TAG}.0.${REVISION}")
# else()
# 	set(FILE_VERSION "${TAG}.${REVISION}")
# endif()

set(FILE_VERSION "${TAG}.0.${REVISION}")
set(INFORMATIONAL_VERSION "${TAG}+${REVISION} [${COMMIT_ID}]")

message(STATUS "File Version: ${FILE_VERSION}")
message(STATUS "Informational Version: ${INFORMATIONAL_VERSION}")

configure_file("${CMAKE_SOURCE_DIR}/AssemblyInfo.cs.in" "${CMAKE_SOURCE_DIR}/Properties/AssemblyInfo.cs")

option(APPVEYOR_CI "Used to write the version info to a file for AppVeyor" OFF)

if(APPVEYOR_CI)
	file(WRITE "${CMAKE_SOURCE_DIR}/version.txt" ${INFORMATIONAL_VERSION})
endif()

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