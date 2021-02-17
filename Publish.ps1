$dirs = Get-ChildItem .\ -include bin,obj,packages,output,Download -Recurse

foreach ($dir in $dirs) 
{ 
	Write-Host "Removing $dir"
	Remove-Item $dir.FullName -Force -Recurse 
}

$myBuildNumber = $(get-date).ToString("yyyy.MM.dd.HHmm");

dotnet publish .\MyAccess.Web\MyAccess.Web.csproj /property:Version=$myBuildNumber --output Output

pause