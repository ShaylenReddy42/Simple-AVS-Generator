# Simple AVS Generator

[![Build Status](https://dev.azure.com/Shaylen/Personal/_apis/build/status/SimpleAVSGenerator?branchName=master)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) [![Azure Pipelines Tests](https://img.shields.io/azure-devops/tests/Shaylen/Personal/2)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master) [![Code Coverage](https://img.shields.io/azure-devops/coverage/Shaylen/Personal/2?label=Code%20Coverage)](https://dev.azure.com/Shaylen/Personal/_build/latest?definitionId=2&branchName=master)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=ShaylenReddy42_Simple-AVS-Generator)

## What's the purpose of this project ?

This project was created to make my life easier

It creates scripts to re-encode videos using [AviSynth+](https://github.com/AviSynth/AviSynthPlus/releases)

The generated AviSynth+ scripts make use of the [L-SMASH-Works plugin](https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/releases) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

## Tools required in your path

| Tool | Category | Filename |
| :--: | :------: | :------: |
| [avs2pipemod](https://github.com/chikuzen/avs2pipemod/releases) | AviSynth+          | avs2pipemod.exe |
| [AVSMeter](https://www.videohelp.com/software/AVSMeter)         | AviSynth+          | AVSMeter64.exe  |
| [x264](https://www.videohelp.com/software/x264-Encoder)         | Video Encoders     | x264.exe        |
| [x265](https://jeremylee.sh/bins/x265.7z)                       | Video Encoders     | x265.exe        |
| [AOM](https://jeremylee.sh/bins/aom.7z)                         | Video Encoders     | aomenc.exe      |
| [qaac](https://github.com/nu774/qaac/releases)*                 | Audio Encoders     | qaac64.exe      |
| [opustools](https://jeremylee.sh/bins/opus.7z)                  | Audio Encoders     | opusenc.exe     |
| [GPAC](https://gpac.wp.imt.fr/downloads/gpac-nightly-builds/)   | Multiplexing Tools | mp4box.exe      |
| [MKVToolNix](https://www.videohelp.com/software/MKVToolNix)     | Multiplexing Tools | mkvmerge.exe    |

* qaac relies on CoreAudioToolbox.dll which is installed with the standalone [iTunes](https://www.videohelp.com/software/iTunes) installer

## Tips

* Personally, I use [PotPlayer](https://www.videohelp.com/software/PotPlayer) to play the scripts
* If for some reason the scripts were originally playing and it doesn't anymore, a Windows update probably caused that, reinstall AviSynth+ to get it to play again

## Required setup

* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) 17.7.3 or later
* [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) 7.0.400
* [CMake](https://cmake.org/download/) 3.21.4 or later
* [Inno Setup](https://jrsoftware.org/isdl.php) 6.2.2 or later

## Build instructions

Run `scripts/build.cmd` and the resulting executable will be in the publish folder at the root of the repository

## Optionally create an installer after building

Run `scripts/create-installer.cmd` and the installer will be in the installer folder at the root of the repository
