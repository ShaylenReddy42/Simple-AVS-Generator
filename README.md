# Simple AVS Generator

[![Build Status](https://dev.azure.com/Shaylen/Personal/_apis/build/status/SimpleAVSGenerator?branchName=master)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) [![Azure Pipelines Tests](https://img.shields.io/azure-devops/tests/Shaylen/Personal/2)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) [![Code Coverage](https://img.shields.io/azure-devops/coverage/Shaylen/Personal/2?label=Code%20Coverage)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=ShaylenReddy42_Simple-AVS-Generator)

## What's the purpose of this project ?

This project was created to make my life easier

It creates scripts to re-encode videos using [AviSynth+](https://github.com/AviSynth/AviSynthPlus/releases)

The generated AviSynth+ scripts make use of the [L-SMASH-Works plugin](https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/releases) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

## Tools required in your path

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

## Required filenames in your path

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

## Tips

* Personally, I use [PotPlayer](https://www.videohelp.com/software/PotPlayer) to play the scripts
* If for some reason the scripts were originally playing and it doesn't anymore, a Windows update probably caused that, reinstall AviSynth+ to get it to play again

## Required setup

* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) 17.4 or later
* [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) 7.0.100
* [CMake](https://cmake.org/download/) 3.21.4 or later
* [Inno Setup](https://jrsoftware.org/isdl.php) 6.2.1 or later

## Build instructions

Run `scripts/build.cmd` and the resulting executable will be in the publish folder at the root of the repository

## Optionally create an installer after building

Run `scripts/create-installer.cmd` and the installer will be in the installer folder at the root of the repository
