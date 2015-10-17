$ErrorActionPreference = "Stop"

$packageVersion = ($env:PackageVersion)
$configuration = ($env:Configuration)
$msBuildExe = ($env:MsBuildExe)
$msBuildTarget = ($env:Targets)

& "$PSScriptRoot\.nuget\nuget.exe" restore "$PSScriptRoot\Ninject.Extensions.UnitOfWork.sln"
if ($LASTEXITCODE -ne 0){
    throw "nuget restore failed"
}

& "$msBuildExe" "$PSScriptRoot\Ninject.Extensions.UnitOfWork.sln" /t:"$msBuildTarget" /p:Configuration="$configuration"
if ($LASTEXITCODE -ne 0){
    throw "sbuild failed"
}

& "$PSScriptRoot\.nunit\nunit-console.exe" "$PSScriptRoot\Ninject.Extensions.UnitOfWork.Tests\bin\$configuration\Ninject.Extensions.UnitOfWork.Tests.dll" /exclude=ExternalDatabase /noshadow  /framework:v4.5
if ($LASTEXITCODE -ne 0){
    throw "tests failed"
}

& "$PSScriptRoot\.nuget\nuget.exe" pack "$PSScriptRoot\Ninject.Extensions.UnitOfWork\Ninject.Extensions.UnitOfWork.csproj" -OutputDirectory "$PSScriptRoot\Releases" -Version "$packageVersion" -Properties configuration="$configuration" -Symbols -Verbosity detailed
if ($LASTEXITCODE -ne 0){
    throw "packing failed"
}