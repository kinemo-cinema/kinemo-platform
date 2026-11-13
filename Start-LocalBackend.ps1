$ErrorActionPreference = 'Stop'
$localDotnet = Join-Path $PSScriptRoot '.local/dotnet10/dotnet.exe'
$dotnetCommand = if (Test-Path -LiteralPath $localDotnet) { $localDotnet } else { (Get-Command dotnet -ErrorAction Stop).Source }
$previousRoot = $env:DOTNET_ROOT
$previousRootX64 = $env:DOTNET_ROOT_X64
if ($dotnetCommand -eq $localDotnet) {
    $env:DOTNET_ROOT = Split-Path -Parent $localDotnet
    $env:DOTNET_ROOT_X64 = $env:DOTNET_ROOT
}
Push-Location $PSScriptRoot
try {
    & $dotnetCommand run --project KinemoPlatform.API --launch-profile http
    if ($LASTEXITCODE -ne 0) { throw "El backend terminó con código $LASTEXITCODE." }
}
finally {
    Pop-Location
    $env:DOTNET_ROOT = $previousRoot
    $env:DOTNET_ROOT_X64 = $previousRootX64
}
