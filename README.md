\coreDMS.DBCreation\dotnet run -- -s C:\dev\coreDMS\sql -f C:\dev\coreDMS\coreDMS.DBCreation\DMS.db

# Scaffolding database

dotnet ef dbcontext scaffold "Datasource=C:\dev\coreDMS\coreDMS.DBCreation\DMS.db" Microsoft.EntityFrameworkCore.Sqlite