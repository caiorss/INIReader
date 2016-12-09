#!/usr/bin/env sh

pkgurl=https://raw.githubusercontent.com/caiorss/INIReader/release/INIReader.1.0.0.nupkg 

curl -O -L $pkgurl

nuget.exe install FParsec -Version 1.0.2   -o ./packages -ExcludeVersion
nuget.exe install INIReader -Source $(pwd) -o ./packages -ExcludeVersion
