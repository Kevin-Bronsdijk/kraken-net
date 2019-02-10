@echo off
cls
dotnet tool install fake-cli --tool-path "tools\fake"
"tools\fake\Fake.exe" run build2.fsx
pause