# Scaffolding database

## Windows

	cd \CoreDMS.DBCreation
	dotnet run -- -s C:\dev\CoreDMS\sql -f C:\dev\CoreDMS\CoreDMS.DBCreation\DMS.db

## Mac/*nix

	cd \CoreDMS.DBCreation
	dotnet run -- -s /mydev/CoreDMS/sql -f /mydev/CoreDMS/CoreDMS.DBCreation/DMS.db

# Building application

	dotnet publish --configuration Release /p:AssemblyVersion:<insert version here>

# Starting application

Directories for uploaded and processed files must be created manually before starting the application.

Create a copy of `appsettings.template.json` and it `appsettings.json` and adjust the values according to environment.
	
	dotnet CoreDMS.dll

# Running as Windows service

As different NuGet packages are not compatible with using target framework `net461` there is right now no way to built a native Window Service.

Propably when .NET Core 2.1 is released native Windows services support should be available according to [this](https://github.com/aspnet/Hosting/issues/904) issue on Github.

In the meantine you can use [NSSM](http://nssm.cc/) which enables you to run almost any .EXE file as a service. To get started simply download NSSM and follow the instructions of NSSM.

Building the binaries can be done using the command:

	dotnet publish -c Release -r win10-x64

Afterwards the created files should be copied to the directory from where the service should be executed and the `appsettings.json` should be changed according to the environment.

## Installing Windows service

	mkdir C:\CoreDMS
	cd CoreDMS
	New-Item -Name service.log -ItemType File # create empty file so stdout and stderr can be redirected
	.\nssm.exe install CoreDMS C:\CoreDMS\CoreDMS.exe
	.\nssm.exe set CoreDMS AppDirectory C:\CoreDMS
	.\nssm.exe set CoreDMS AppStdout C:\CoreDMS\service.log 
	.\nssm.exe set CoreDMS AppStderr C:\CoreDMS\service.log

	.\nssm.exe set CoreDMS AppEnvironmentExtra COREDMS_LogDir=C:\CoreDMS\logs\
	.\nssm.exe set CoreDMS AppEnvironmentExtra COREDMS_Uploads=C:\DMS\upload\
	.\nssm.exe set CoreDMS AppEnvironmentExtra COREDMS_Processed=C:\DMS\out\
	.\nssm.exe set CoreDMS AppEnvironmentExtra COREDMS_DbFile=C:\DMS\dev_database.sqlite
	.\nssm.exe set CoreDMS AppEnvironmentExtra COREDMS_Applicationurl=http://192.168.178.204:3000

## Removing Windows

	.\nssm.exe remove CoreDMS