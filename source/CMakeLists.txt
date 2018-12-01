cmake_minimum_required(VERSION 3.8)

include(CSharpUtilities)

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

csharp_set_windows_forms_properties(
	"Simple AVS Generator.cs"
	"Simple AVS Generator.Designer.cs"
	"Simple AVS Generator.resx"
)

csharp_set_designer_cs_properties(
	Properties/Resources.resx
	Properties/Resources.Designer.cs
	Properties/Settings.settings
	Properties/Settings.Designer.cs
)

set_target_properties(
	${PROJECT_NAME} PROPERTIES
	VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
	VS_DOTNET_REFERENCES "System;System.Windows.Forms;System.Data;System.Drawing"
	OUTPUT_NAME "Simple AVS Generator"
)