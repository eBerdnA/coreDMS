$currentDirectory = pwd
cd coreDMS
Write-Host "- cleaning .\obj"
$pathToClean = "$($currentDirectory)\coreDMS\obj"
Get-ChildItem -Path $pathToClean -Include * -File -Recurse | foreach { $_.Delete() }
Write-Host "- cleaning solution"
dotnet clean
Write-Host "- publishing solution"
dotnet publish -c Release -r win10-x64
cd $currentDirectory
Write-Host "- creating zip file"
$buildDateTime = Get-Date
$buildDate = $buildDateTime.ToString("yyyyMMdd")
$buildTime = $buildDateTime.ToString("HHmm")
$source = "$($currentDirectory)\coreDMS\bin\Release\netcoreapp2.0\win10-x64\publish"
$destination = "$($currentDirectory)\coreDMS\bin\Release\netcoreapp2.0\CoreDMS_$($buildDate)_$($buildTime).zip"

If(Test-path $destination) {
	Write-Host "- publish zip $($destination) file already exists"
	exit
}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($Source, $destination) 
Write-Host "- created zip file: $($destination)"
