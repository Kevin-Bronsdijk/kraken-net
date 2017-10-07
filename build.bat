@echo off
cls
"tools\NuGet\NuGet.exe" "restore" "src\kraken-net-v2\kraken-net-v2.csproj"
"tools\NuGet\NuGet.exe" "restore" "src\kraken-net-v2-tests\kraken-net-v2-tests.csproj"
"tools\NuGet\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\Fake\tools\Fake.exe" build.fsx
pause