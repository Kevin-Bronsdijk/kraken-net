@echo off
cls
"tools\NuGet\NuGet.exe" "Install" "NUnit.ConsoleRunner" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\NuGet\NuGet.exe" "Install" "NUnit" "-OutputDirectory" "tools" "-ExcludeVersion"
dotnet restore
dotnet tool install fake-cli --tool-path "tools\fake"
"tools\fake\Fake.exe" run build2.fsx
pause