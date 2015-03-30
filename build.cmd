@echo off

echo Installing build tools...
".\tools\nuget\NuGet.exe" "Install" "FAKE.Core" "-OutputDirectory" "tools" "-ExcludeVersion" "-version" "3.23.0"
".\tools\nuget\NuGet.exe" "Install" "NUnit.Runners" "-OutputDirectory" "tools" "-ExcludeVersion" "-version" "2.6.4"
".\tools\nuget\NuGet.exe" "Install" "SourceLink.Fake" "-OutputDirectory" "tools" "-ExcludeVersion" "-version" "0.4.2"

set TARGET="BuildApp"
if not [%1]==[] (set TARGET="%1")

set BUILDMODE="Release"
if not [%2]==[] (set BUILDMODE="%2")

"tools\FAKE.Core\tools\Fake.exe" build.fsx "target=%TARGET%" "buildMode=%BUILDMODE%"

:Quit
exit /b %errorlevel%