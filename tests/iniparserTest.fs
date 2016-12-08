module IniParserTest

  // #if INTERACTIVE
    // #r "../packages/NUnit/lib/dotnet/nunit.framework.dll"
    // #r "../packages/FsUnit/lib/net45/FsUnit.NUnit.dll"
    // #r "../bin/Debug/INIReader.dll"
    // #endif 
    
open NUnit.Framework
open FsUnit
open INIReader 

let inidata = """
[server]
# host the server will listen to.
host = 0.0.0.0          # it will listen to any host
port = 8080             # server port 
path = /home/arch/data  

name = "John McArthur"  # User name 
role = admin 

[window]
width       = 600
height      = 350
position.x  = 400
position.y  = 200

[references]
# commentary 
dependencies = [(Nuget.Core 10.23) (FParsec 0.945)]
values = [10 20 30 40]
# comment lines 
#
target = net45

tuple = (10 hello 20.100.200 "hello world")
"""



let getAst () = INIParser.read  inidata

[<Test>]
let ``The value of section 'window' and key 'position.x' is 400`` () =
    // let ast = getAst ()
    INIExtr.fieldString "window" "position.x" (getAst()) |>  should equal (Some "400")

[<Test>]
let ``The value of section 'references' and key 'target' is 'net45'`` () =
    INIExtr.fieldString "references" "target" (getAst()) |>  should equal (Some "net45")

[<Test>]
let ``The value of section 'references' and key 'dependencies' is = `` () =
    // let ast = getAst ()
    INIExtr.fieldListOfTuples "references" "dependencies" (getAst())
    |>  should equal (Some [["Nuget.Core"; "10.23"] ; ["FParsec" ; "0.945"]])



