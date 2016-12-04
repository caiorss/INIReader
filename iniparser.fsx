

#I "packages/FParsec.1.0.2/lib/net40-client" 
#r "FParsec.dll" 
#r "FParsecCS.dll"

open System

open FParsec

type INIKey = string

type INIValue =
    | INIString of string
    | INITuple  of INIValue list 
    | INIList   of INIValue list
    | INIEmpty 


type INIData = {
    section: string ;
    payload: INIKey * INIValue) list; 
    }


// Parses text surrounded by zero or more white spaces    
let ws p = spaces >>. p .>> spaces

let wstr t = ws (pstring t)

let betweenSquareBrackets p =
    (wstr "[") >>. p .>> (wstr "]")

let betweenParenthesis p =
    wstr "(" >>. p .>> wstr ")"


let anyText s = manySatisfy (fun c -> true) s

let identifier<'T> : Parser<string, 'T> =
     many1Satisfy2 (fun ch -> Char.IsLetter(ch)) (fun ch -> Char.IsLetterOrDigit(ch))


let anyText2 s =
    many1Satisfy (fun ch -> (not <| Char.IsWhiteSpace(ch))
                             && not ( ch = ')'
                                      ||  ch = '('
                                      ||  ch = ']'
                                      ||  ch = '['
                                      ||  ch = ','
                                      ||  ch = ';'
                                      )
                  ) s
    

let parseSection<'T> : Parser<string, 'T> = squareBrackets identifier


(*     
     > run quoted "\"hello world\"" ;;
     val it : ParserResult<string,unit> = Success: "hello world"
     >      

*)   
let quoted<'T> : Parser<string,'T> =
    pchar '"' >>. manySatisfy (fun c -> c <> '"') .>> pchar '"'


let comment<'T> : Parser<unit, 'T> =
    pstring "#" >>. skipRestOfLine true ;;



let testData1 = "hosts = 192.168.12 "
let testData2 = "refs = [(Nuget.Core, 1.12), (Fsharp.Charting, 1.23)]"
let testData3 = "mydata = [\"hello world\", something, nothing, 2000]"


let parseINIString<'T> : Parser<INIValue, 'T> =
    quoted <|> anyText2 |>> INIString

let parseINITuple<'T> : Parser<INIValue, 'T> =
    betweenParenthesis <| many (parseINIString .>> spaces .>> optional (pchar ',' ))
    |>> INITuple

let parseINIList<'T> : Parser<INIValue, 'T> =
    betweenSquareBrackets <| many ( parseINIString <|> parseINITuple
                                    .>> spaces
                                    .>> optional (pchar ';' ))
    |>> INIList


let parseINIValue<'T> : Parser<INIValue, 'T> =  
    parseINIList
    <|> parseINITuple
    <|> parseINIString
    <|> (skipMany comment >>. spaces |>> (fun _ -> INIEmpty))
    .>> (skipMany comment)
                 
                 
let parseKV<'T> : Parser<string * INIValue, 'T> =
    identifier .>> wstr "=" .>>. parseINIValue 

let pcomment<'T> : Parser<char, 'T> =
    pchar '#'
    >>. skipManySatisfy (fun c -> c <> '\n' || c <> '\r')
    >>. (pchar '\n' <|> pchar '\r')


let peol<'T> : Parser<char, 'T> = pcomment <|> (pchar '\n' <|> pchar '\r')

let applyPlines pstatement = many (spaces >>. pstatement .>> peol) .>> eof

let parseSection<'T> : Parser<INIData, 'T> =
    betweenSquareBrackets identifier
    .>>. many (parseKV .>> spaces)
    |>> fun (a, b) -> { section = a ; payload = b}


let parseSection2<'T> : Parser<INIData, 'T> =
    betweenSquareBrackets identifier
    .>>. many (skipMany comment >>. parseKV .>> spaces)
    |>> fun (a, b) -> { section = a ; payload = b}
              
let parseINI<'T> : Parser<(INIData list), 'T> =
    applyPlines parseSection2

// Doesn't work.
let parseINISection<'T> : Parser<(INIData list), 'T> =
    many ( (skipMany <| optional pcomment)
           >>. betweenSquareBrackets identifier
           .>>. many (parseKV .>> spaces)
           |>> fun (a, b) -> { section = a ; payload = b}
           )


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
version    = 1.2.3
empty      = 
"""



    


// let parseINIValue =   quoted
//                   <|> 

                  
                 
