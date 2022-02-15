# SimpleAVSGenerator [![Build status](https://ci.appveyor.com/api/projects/status/61g0g8mca7ihkkot?svg=true)](https://ci.appveyor.com/project/Shaylen/simple-avs-generator) #

## What's This Project For? ##

This project was created to make my life easier

It creates scripts to re-encode videos using [AviSynth+](https://github.com/AviSynth/AviSynthPlus/releases)

The generated AviSynth+ scripts make use of the [L-SMASH-Works plugin](https://github.com/HolyWu/L-SMASH-Works/releases) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

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
| [qaac](https://github.com/nu774/qaac/releases) |
| [opustools](https://jeremylee.sh/bins/) |

| Multiplexing Tools |
| :---: |
| [GPAC](https://gpac.wp.imt.fr/downloads/gpac-nightly-builds/) |
| [MKVToolNix](https://www.videohelp.com/software/MKVToolNix) |

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

* [Microsoft Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/) with C# language support
* [.Net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [CMake v3.21.4 or later](https://cmake.org/download/) [This must be in your PATH]

## Build Instructions ##

Run `build.cmd` and the resulting executable will be in the publish folder