(* Script to load the installed NuGet pacakge *)

(* ===== Load the generated library in the REPL ====

*)
#I "packages/FParsec.1.0.2/lib/net40-client" 
#r "FParsec.dll" 
#r "FParsecCS.dll"
#r "packages/INIReader.1.0.0/lib/INIReader.dll"

open FParsec
open INIReader

