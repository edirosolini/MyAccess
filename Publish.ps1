$dirs = Get-ChildItem .\ -include bin,obj,packages,output,Download -Recurse

foreach ($dir in $dirs) 
{ 
	Write-Host "Removing $dir"
	Remove-Item $dir.FullName -Force -Recurse 
}

$myBuildNumber = $(get-date).ToString("yyyy.MM.dd.HHmm");

dotnet restore

dotnet build /property:Version=$myBuildNumber --no-restore

dotnet publish .\WebApplication\MyAccess.WebApplication.csproj /property:Version=$myBuildNumber --no-restore --output Output

pause