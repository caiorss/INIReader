
# =====  User Options ============== #

PLATFORM := anycpu
TYPE     := library 
src := INIReader.fs

# Object code that will be generated 
obj := INIReader.dll

# Dll files to be included 
ASSEMBLIES := FParsec.dll FParsecCS.dll

# List of directories where are the .NET assemblies (*.dll files)
INCLUDES   := packages/FParsec.1.0.2/lib/net40-client

PKGURL := https://raw.githubusercontent.com/caiorss/INIReader/release/INIReader.1.0.0.nupkg

#-------------------------------------#


# F# compiler 
FSC   := fsc

# Nuget executable 
NUGET := nuget.exe

# Output directory with compiled code 
BIN := bin

target := $(BIN)/$(obj)



#===========  Rules ===================#

all: $(target)

$(target): $(src)
	$(FSC) $(src) --platform:$(PLATFORM) \
               --target:$(TYPE) --out:$(target) \
               $(addprefix -I:,$(INCLUDES)) \
               $(addprefix -r:, $(ASSEMBLIES))

# Dependencies necessary to compile
deps:
	$(NUGET) install FParsec -OutputDirectory packages -Version 1.0.2

# Loads the library in the interactive shell 
loader: all
	fsi --use:loader.fsx

# Make Nuget package 
pkg: 
	nuget pack INIReader.nuspec -OutputDirectory ./release

# Show Nuget package 
pkg-show: 
	unzip -l INIReader.1.0.0.nupkg

pkg-install:
	nuget.exe install INIReader -Source ${CURDIR} -o ./packages

## Install package to local repository
pkg-install-local:
	nuget.exe install INIReader -Source ${CURDIR} -o ~/nuget

pkg-install-github:
	curl -O -L $(PKGURL)
	nuget.exe install INIReader -Source $(shell pwd) -o ./packages

pkg-rm:
	rm -rf packages/INIReader.1.0.0

# load installed nuget package into repl. 
pkg-loader:
	fsi --use:pkgloader.fsx

clean:
	rm -rf $(BIN)/*

