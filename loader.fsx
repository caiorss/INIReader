(* ===== Load the generated library in the REPL ====

*)
#I "packages/FParsec.1.0.2/lib/net40-client" 
#r "FParsec.dll" 
#r "FParsecCS.dll"

// #r "bin/INIReader.dll"
#load "INIReader.fs" 

open FParsec
open INIReader


let testDataSection = """
[PROJECT]
# project name 
projectName = helloWorld
# exe for command line app
target     = exe

# The framework is
# - net45 - for .NET 4.5 
# - net40 - for .NET 4.0
# - net30 - for .NET 3.0
framework  = net45

# Dependencies to be installed
#
references = [ (NuGet.Core 0.9.1) (FParsec 1.0.2) (OxyPlot _)]
version    = 1.2.3  # Version of your application
output      =  
"""


let testData10 = """
[INFO]
name       =  "mr dude"
email      = mrdude@gmail.com

[PROJECT]
projectName = helloWorld
target     = exe
framework  = net45
references = [ (NuGet.Core 0.9.1) (FParsec 1.0.2) (OxyPlot _)]
names      = [ package1 package10.2 "package 100" 300 500 ]
atuple     = (100 "hello world" eggs nuts milk shake)
version    = 1.2.3
empty      = 
"""

let testDataFailure = """
[INFO
name       =  "mr dude"
email      = mrdude@gmail.com

PROJECT]
projectName = helloWorld
target     = exe
framework  = net45
references = [ (NuGet.Core 0.9.1) (FParsec 1.0.2) (OxyPlot _)]
version    = 1.2.3
empty      = 
"""

let testData1 = "hosts = 192.168.12 "
let testData2 = "refs = [(Nuget.Core, 1.12), (Fsharp.Charting, 1.23)]"
let testData3 = "mydata = [\"hello world\", something, nothing, 2000]"



let testParse1 () = run INIReader.INIParser.parseINI testData10 
let testParse2 () = run INIReader.INIParser.parseINI testDataSection
let testParse3 () = run INIReader.INIParser.parseINI testDataFailure


let data = INIParser.read testData10 ;;


// type myType = ('a -> option 'b) -> list 'a -> option (list 'b)

