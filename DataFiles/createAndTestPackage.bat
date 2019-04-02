REM save current directory
set startingDirectory=%CD%

REM Build release version of DataFiles project
REM no longer running this from here - do not put a possibly bad build over a good one
REM 
REM CALL compileReleaseProjects.bat

REM Build Nuget package
nuget pack WithoutHaste.DataFiles.nuspec
REM Sign NuGet package
nuget sign WithoutHaste.DataFiles.1.0.0-alpha4.nupkg -CertificatePath E:\Adulting\WithoutHasteLLC\Certificates\Comodo_certificate_backup_PKCS12_from_firefox.p12 -Timestamper http://sha256timestamp.ws.symantec.com/sha256/timestamp

REM Copy package to local test source folder
xcopy *.nupkg ..\..\..\NuGetTestSource

REM Run InstallationTestsSetup
cd ..\InstallationTests
START /WAIT Setup\InstallationTestsSetup\bin\Release\InstallationTestsSetup.exe

REM Install NuGet package in each auto-generated solution
REM This section removed because command-line-nuget will download packages but WILL NOT update the project file
REM
REM set installationTestsDirectory=%CD%
REM set frameworks=20 30 35 40 45 451 452 46 461 462 47 471 472
REM (for %%f in (%frameworks%) do ( 
REM 	cd %installationTestsDirectory%
REM 	cd AutoGenerated\net%%f
REM  	nuget install WithoutHaste.DataFiles -Version 1.0.0-alpha4
REM ))
 
REM Return to starting directory
cd %startingDirectory%