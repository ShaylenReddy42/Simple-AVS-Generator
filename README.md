# Simple AVS Generator #

|              | AppVeyor | Azure Pipelines |
| :----------: | :------: | :-------------: |
| Build Status | [![AppVeyor Build](https://ci.appveyor.com/api/projects/status/85v9q2xcatmdtnv7?svg=true)](https://ci.appveyor.com/project/Shaylen/simple-avs-generator) | [![Azure Pipelines Build](https://dev.azure.com/Shaylen/Personal/_apis/build/status/ShaylenReddy42.Simple-AVS-Generator?branchName=master)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) |
| Tests        | [![AppVeyor Tests](https://img.shields.io/appveyor/tests/Shaylen/simple-avs-generator)](https://ci.appveyor.com/project/Shaylen/simple-avs-generator/build/tests) | [![Azure Pipelines Tests](https://img.shields.io/azure-devops/tests/Shaylen/Personal/2)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) |

[![Code Coverage](https://img.shields.io/azure-devops/coverage/Shaylen/Personal/2?label=Code%20Coverage)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=ShaylenReddy42_Simple-AVS-Generator)

## What's This Project For? ##

This project was created to make my life easier

It creates scripts to re-encode videos using [AviSynth+](https://github.com/AviSynth/AviSynthPlus/releases)

The generated AviSynth+ scripts make use of the [L-SMASH-Works plugin](https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/releases) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

## Tools Required in Your Path ##

| AviSynth+ |
| :---: |
| [avs2pipemod](https://github.com/chikuzen/avs2pipemod/releases) | 
| [AVSMeter64](https://www.videohelp.com/software/AVSMeter) |

| Video Encoders |
| :---: |
| [x264](https://www.videohelp.com/software/x264-Encoder) |
| [x265](https://jeremylee.sh/bins/) |
| [AOM-AV1](https://jeremylee.sh/bins/) |

| Audio Encoders |
| :---: |
| [qaac](https://github.com/nu774/qaac/releases)* |
| [opustools](https://jeremylee.sh/bins/) |

| Multiplexing Tools |
| :---: |
| [GPAC](https://gpac.wp.imt.fr/downloads/gpac-nightly-builds/) |
| [MKVToolNix](https://www.videohelp.com/software/MKVToolNix) |

* qaac relies on CoreAudioToolbox.dll which is installed with the standalone [iTunes](https://www.videohelp.com/software/iTunes) installer

## Required Filenames in Your Path ##

| Tool        | Filename        |
| :---------: | :-------------: |
| avs2pipemod | avs2pipemod.exe |
| AVSMeter    | AVSMeter64.exe  |
| x264        | x264.exe        |
| x265        | x265.exe        |
| AOM         | aomenc.exe      |
| qaac        | qaac64.exe      |
| opustools   | opusenc.exe     |
| GPAC        | mp4box.exe      |
| MKVToolNix  | mkvmerge.exe    |

## Required Setup ##

* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) 17.4 or later
* [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) 7.0.100
* [CMake](https://cmake.org/download/) 3.21.4 or later

## Build Instructions ##

Run `build.cmd` and the resulting executable will be in the publish folder