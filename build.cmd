@echo off

echo Installing build tools...
".\tools\nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion" "-version" "3.10.0"

set TARGET="BuildApp"
if not [%1]==[] (set TARGET="%1")

set BUILDMODE="Release"
if not [%2]==[] (set BUILDMODE="%2")

"packages\FAKE\tools\Fake.exe" build.fsx "target=%TARGET%" "buildMode=%BUILDMODE%"

:Quit
exit /b %errorlevel%