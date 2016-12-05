
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

clean:
	rm -rf $(BIN)/*
