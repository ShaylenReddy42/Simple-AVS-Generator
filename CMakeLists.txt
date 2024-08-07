cmake_minimum_required(VERSION 3.29.0)

include(FindGit)

project(
	SimpleAVSGenerator
	LANGUAGES NONE
)

# Extracting version

set(TAG "0.0.0")
set(REVISION "0")
set(COMMIT_ID "unknown")

if(${GIT_FOUND} AND EXISTS "${CMAKE_SOURCE_DIR}/.git")
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
		set(TAG "0.0.0")
		
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

set(FILE_VERSION "${TAG}.${REVISION}")
set(INFORMATIONAL_VERSION "${TAG}+${REVISION}-${COMMIT_ID}")

message(STATUS "File Version: ${FILE_VERSION}")
message(STATUS "Informational Version: ${INFORMATIONAL_VERSION}")

configure_file(
	"${CMAKE_SOURCE_DIR}/cmake/SimpleAVSGenerator.WinForms.AssemblyInfo.cs.in" 
	"${CMAKE_SOURCE_DIR}/src/Frontends/SimpleAVSGenerator.WinForms/Properties/AssemblyInfo.cs")

configure_file(
	"${CMAKE_SOURCE_DIR}/cmake/SimpleAVSGeneratorCore.AssemblyInfo.cs.in" 
	"${CMAKE_SOURCE_DIR}/src/Libraries/SimpleAVSGeneratorCore/Properties/AssemblyInfo.cs")

configure_file(
	"${CMAKE_SOURCE_DIR}/cmake/installer.iss.in" 
	"${CMAKE_SOURCE_DIR}/scripts/installer.iss")

option(CI_BUILD "Used to write the version info to a file for CI" OFF)

if(CI_BUILD)
	file(WRITE "${CMAKE_SOURCE_DIR}/version.txt" ${INFORMATIONAL_VERSION})
endif()