
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
