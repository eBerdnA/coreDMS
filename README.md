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