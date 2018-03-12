\coreDMS.DBCreation\dotnet run -- -s C:\dev\coreDMS\sql -f C:\dev\coreDMS\coreDMS.DBCreation\DMS.db

# Scaffolding database

dotnet ef dbcontext scaffold "Datasource=C:\dev\coreDMS\coreDMS.DBCreation\DMS.db" Microsoft.EntityFrameworkCore.Sqlite

// TODO How to publish including 'wwwroot'

dbFile - path to sqlite file
uploads - directory where uploads should be stored
processed - directory where process files should be stored


dotnet publish --configuration Release

directories for uploaded and processed files must be created manually before starting the application

set dbFile="C:\dev\demoDotnetCore\dev_database.sqlite"
set uploads=C:\dev\coreDMS\coreDMS\bin\Release\netcoreapp2.0\publish\wwwroot\uploads
set processed=C:\dev\coreDMS\coreDMS\bin\Release\netcoreapp2.0\publish\wwwroot\processed
dotnet CoreDMS.dll