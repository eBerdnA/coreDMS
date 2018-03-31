# Scaffolding database

	cd \coreDMS.DBCreation
	dotnet run -- -s C:\dev\coreDMS\sql -f C:\dev\coreDMS\coreDMS.DBCreation\DMS.db

# Building application

	dotnet publish --configuration Release

# Starting application

Directories for uploaded and processed files must be created manually before starting the application.

	set dbFile="C:\dev\demoDotnetCore\dev_database.sqlite"
	set uploads=C:\dev\coreDMS\coreDMS\bin\Release\netcoreapp2.0\publish\wwwroot\uploads
	set processed=C:\dev\coreDMS\coreDMS\bin\Release\netcoreapp2.0\publish\wwwroot\processed
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

## Removing Windows

	.\nssm.exe remove CoreDMS