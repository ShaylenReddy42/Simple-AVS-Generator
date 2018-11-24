cmake_minimum_required(VERSION 3.8)

project(
	SimpleAVSGenerator
	LANGUAGES CSharp
)

add_executable(
	${PROJECT_NAME} WIN32
	Program.cs
	"Simple AVS Generator.cs"
	"Simple AVS Generator.Designer.cs"
	"Simple AVS Generator.resx"
	Properties/AssemblyInfo.cs
	Properties/Resources.resx
	Properties/Resources.Designer.cs
	Properties/Settings.settings
	Properties/Settings.Designer.cs
)

set_source_files_properties(
	"Simple AVS Generator.Designer.cs" PROPERTIES
	VS_CSHARP_DependentUpon "Simple AVS Generator.cs"
	VS_CSHARP_SubType "Form"
)

set_source_files_properties(
	"Simple AVS Generator.resx" PROPERTIES
	VS_CSHARP_DependentUpon "Simple AVS Generator.cs"
)

set_source_files_properties(
	Resources.Designer.cs PROPERTIES
	VS_CSHARP_DependentUpon Resources.resx
)

set_source_files_properties(
	Settings.Designer.cs PROPERTIES
	VS_CSHARP_DependentUpon Settings.settings
)

set_target_properties(
	${PROJECT_NAME} PROPERTIES
	VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
	VS_DOTNET_REFERENCES "System;System.Windows.Forms;System.Data;System.Drawing"
	OUTPUT_NAME "Simple AVS Generator"
)