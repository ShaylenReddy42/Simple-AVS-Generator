# Simple AVS Generator [![Build status](https://ci.appveyor.com/api/projects/status/61g0g8mca7ihkkot?svg=true)](https://ci.appveyor.com/project/Shaylen/simple-avs-generator) #

## What is this project for? ##

This project was created to make my life easier

It creates scripts to re-encode videos using [AviSynth+](https://github.com/pinterf/AviSynthPlus/releases)

The generated AviSynth+ scripts make use of the [LSMASHSource msvc plugins](https://www.dropbox.com/sh/3i81ttxf028m1eh/AAABkQn4Y5w1k-toVhYLasmwa?dl=0) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

### Tools required in your path ###

#### AviSynth+ related tools ####

* [avs2pipemod](https://github.com/chikuzen/avs2pipemod/releases)
* [AVSMeter64](https://www.videohelp.com/software/AVSMeter)

#### Video encoders ####

* [x264](https://download.videolan.org/pub/x264/binaries/)
* [x265](http://msystem.waw.pl/x265/)
* aomenc [[Build from source](https://aomedia.googlesource.com/aom/)]

#### Audio encoders ####

* [qaac64](https://github.com/nu774/qaac/releases)
* [opusenc](https://opus-codec.org/downloads/)

#### Multiplexing tools ####

* [mp4box from GPAC](https://gpac.wp.imt.fr/downloads/gpac-nightly-builds/)
* [mkvmerge from MKVToolNix](https://www.videohelp.com/software/MKVToolNix)


## How do I get set up? ##

* [Microsoft Visual Studio 2017 Community Edition](https://visualstudio.microsoft.com/vs/community/) with C# language support
* [CMake v3.8 or later](https://cmake.org) [This must be in your PATH]

## Build Instructions ##

Run build/Build [Project].cmd and the resulting executable will be in same folder as the script