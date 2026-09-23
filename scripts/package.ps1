[CmdletBinding()]
param(
    [string]$RimWorldDir
)

$ErrorActionPreference = 'Stop'

$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$projectPath = Join-Path $repositoryRoot 'Source\BuffWatermill\BuffWatermill.csproj'
$packageRoot = Join-Path $repositoryRoot 'dist\BuffWatermill'
$assemblyPath = Join-Path $packageRoot '1.6\Assemblies\BuffWatermill.dll'
$aboutSource = Join-Path $PSScriptRoot 'About.xml'
$previewSource = Join-Path $PSScriptRoot 'thumnails.jpg'

$buildArguments = @('build', $projectPath, '-c', 'Release')
if ($RimWorldDir) {
    $resolvedRimWorldDir = [System.IO.Path]::GetFullPath($RimWorldDir)
    $buildArguments += "-p:RimWorldDir=$resolvedRimWorldDir"
}

if (-not (Test-Path -LiteralPath $aboutSource -PathType Leaf)) {
    throw "About.xml was not found: $aboutSource"
}

if (-not (Test-Path -LiteralPath $previewSource -PathType Leaf)) {
    throw "Thumbnail image was not found: $previewSource"
}

if (Test-Path -LiteralPath $packageRoot) {
    Remove-Item -LiteralPath $packageRoot -Recurse -Force
}

Write-Host 'Building Buff Watermill...'
& dotnet @buildArguments
if ($LASTEXITCODE -ne 0) {
    throw "Release build failed with exit code $LASTEXITCODE."
}

if (-not (Test-Path -LiteralPath $assemblyPath -PathType Leaf)) {
    throw "Built assembly was not found: $assemblyPath"
}

$aboutDestination = Join-Path $packageRoot 'About'
New-Item -ItemType Directory -Path $aboutDestination -Force | Out-Null
$aboutPath = Join-Path $aboutDestination 'About.xml'
Copy-Item -LiteralPath $aboutSource -Destination $aboutPath

Add-Type -AssemblyName System.Drawing
$previewPath = Join-Path $aboutDestination 'Preview.png'
$previewImage = [System.Drawing.Image]::FromFile($previewSource)
try {
    $previewImage.Save($previewPath, [System.Drawing.Imaging.ImageFormat]::Png)
}
finally {
    $previewImage.Dispose()
}

Write-Host "Package created: $packageRoot"
