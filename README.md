# Simple AVS Generator #

## What is this project for? ##

This project was created to make my life easier

It creates scripts to re-encode videos using AviSynth+, x264, x265, qaac64 and opusenc [with my preferred settings] and using mp4box or mkvmerge to mux the resulting files into a container.
This project currently assumes that all these tools are in your PATH and easily accessible

The generated AviSynth+ scripts make use of the [LSMASHSource msvc plugins](https://www.dropbox.com/sh/3i81ttxf028m1eh/AAABkQn4Y5w1k-toVhYLasmwa?dl=0) which must be placed in the relevant AviSynth+ plugin folders [plugins+ for the 32bit dll and plugins64+ for the 64bit]

## How do I get set up? ##

* [Microsoft Visual Studio 2017 Community Edition](https://visualstudio.microsoft.com/vs/community/) with C# language support
* [CMake v3.8 or later](https://cmake.org) [This must be in your PATH]

## Build Instructions ##

Run build/Build [Project].cmd and the resulting executable will be in same folder as the script