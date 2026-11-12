$ErrorActionPreference = 'Stop'
$mysqlRoot = Join-Path $PSScriptRoot '.local/mysql'
$mysqlServer = Join-Path $mysqlRoot 'mysql-8.4.13-winx64/bin/mysqld.exe'
if (-not (Test-Path -LiteralPath $mysqlServer)) {
    throw 'No se encuentra el runtime local de MySQL en .local/mysql.'
}
$listener = Get-NetTCPConnection -LocalPort 3306 -State Listen -ErrorAction SilentlyContinue
if ($listener) {
    Write-Host 'El puerto 3306 ya está ocupado. No se inició otro servidor.'
    return
}
$mysqlArguments = @(
    '--no-defaults',
    ('--basedir="' + (Join-Path $mysqlRoot 'mysql-8.4.13-winx64') + '"'),
    ('--datadir="' + (Join-Path $mysqlRoot 'data') + '"'),
    '--bind-address=127.0.0.1',
    '--port=3306',
    '--mysqlx=OFF',
    ('--log-error="' + (Join-Path $mysqlRoot 'mysql.log') + '"')
)
$mysqlProcess = Start-Process -FilePath $mysqlServer -ArgumentList $mysqlArguments -WindowStyle Hidden -PassThru
Write-Host "MySQL iniciado en segundo plano (PID $($mysqlProcess.Id)). Log: $mysqlRoot/mysql.log"
