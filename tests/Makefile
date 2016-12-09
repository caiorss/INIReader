# =====  User Options ============== #


PLATFORM := anycpu
TYPE     := library
src      := iniparserTest.fs

FSC = fsc

NUNIT := packages/NUnit.ConsoleRunner/tools/nunit3-console.exe

# Object code that will be generated
obj := iniparserTest.dll


# List of directories where are the .NET assemblies (*.dll files)
INCLUDES   :=    ../bin/Debug

# Dll files to be included
ASSEMBLIES := ../packages/FParsec.1.0.2/lib/net40-client/FParsec.dll    \
		  	  ../packages/FParsec.1.0.2/lib/net40-client/FParsecCS.dll  \
		  	  packages/FsUnit/lib/net45/FsUnit.NUnit.dll             \
			  packages/NUnit/lib/net45/nunit.framework.dll          \
			  ../bin/Debug/FS.INIReader.dll


#======= Don't Change ============ #

# Output directory with compiled code
BIN := bin

target := $(BIN)/$(obj)



#===========  Rules ===================#

all: $(target)

## Install dependencies
## 
$(NUNIT): 
	 nuget.exe install NUnit.Runners  -OutputDirectory packages -ExcludeVersion -Prerelease 
	 nuget.exe install NUnit          -OutputDirectory packages -ExcludeVersion -Prerelease 
	 nuget.exe install FsUnit         -OutputDirectory packages -ExcludeVersion -Prerelease 

nunit: $(NUNIT)


$(target): $(src) nunit
	$(FSC) $(src) --platform:$(PLATFORM) \
               --target:$(TYPE) --out:$(target) \
               $(addprefix -r:, $(ASSEMBLIES))

	cp -v $(ASSEMBLIES) $(BIN)


## Run the unit tests
## 
test: nunit  $(target) 
	cd ./bin && ../$(NUNIT) iniparserTest.dll 

clean:
	rm -rf bin/*

## Clean all binary files and packages
clean-all:
	rm -rf bin/* packages/* 
