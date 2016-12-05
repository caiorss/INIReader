#if INTERACTIVE
#I "packages/FParsec.1.0.2/lib/net40-client" 
#r "FParsec.dll" 
#r "FParsecCS.dll"
#endif 


open System
open FParsec

type INIKey = string

type INIValue =
    | INIString of string
    | INITuple  of INIValue list 
    | INIList   of INIValue list
    | INIEmpty 

type INIData = Map<string, Map<INIKey,INIValue>>


(* ===========================  Primitive Parsers  ================================== *)

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



(* ===========================  INIParser ====================================== *)

// module FParser = 
// open INIAst    
// open PrimitiveParsers


let parseQuoted<'T> : Parser<string,'T> =  pchar '"' >>. manySatisfy (fun c -> c <> '"') .>> pchar '"'

let extracts p str =
    match run p str with
    | Success (result, _, _)  ->  result
    | Failure (msg,    _, _)  ->  failwith msg


let extract2 p str =
    match run p str with
    | Success (result, _, _)  ->  Some result
    | Failure (msg,    _, _)  ->  None


// let parseSection<'T> : Parser<string, 'T> = betweenSquareBrackets identifier


let comment<'T> : Parser<unit, 'T> =
    pstring "#" >>. skipRestOfLine true ;;

let parseINIString<'T> : Parser<INIValue, 'T> =                  
    parseQuoted <|> anyText2 |>> INIString

let parseINITuple<'T> : Parser<INIValue, 'T> =
    betweenParenthesis <| many (parseINIString .>> spaces .>> optional (pchar ',' ))
    |>> INITuple

let parseINIList<'T> : Parser<INIValue, 'T> =
    betweenSquareBrackets <| many ( parseINIString <|> parseINITuple
                                    .>> spaces
                                    .>> optional (pchar ';' ))
    |>> INIList


let parseINIValue<'T> : Parser<INIValue, 'T> =  
   parseINIList <|> parseINITuple
   <|> parseINIString
   <|> (skipMany comment >>. spaces |>> (fun _ -> INIEmpty))
   .>> (skipMany comment)


let parseKV<'T> : Parser<string * INIValue, 'T> =
    identifier .>> wstr "=" .>>. parseINIValue

// let parseKV s () = (identifier .>> wstr "=" .>>. parseINIValue) s


let parseSection<'T> :  Parser<(string * Map<string, INIValue>),'T> =
    betweenSquareBrackets identifier .>>. (many (skipMany comment >>. parseKV .>> spaces) |>> Map.ofList)


let parseINI<'T> : Parser<INIData, 'T> =   many parseSection |>> Map.ofList 

                  
// [<EntryPoint>]
// let main argv =
//     printfn "Started"
//     0
