param(
  [string]$Version = "vNEXT",
  [switch]$Prerelease
)

$ErrorActionPreference = 'Stop'

# Build publish
$rid = 'win-x64'
$outDir = "publish/$rid-$Version"
$zipPath = "publish/TypeLibExporter_NET8_${Version}_${rid}.zip"

Write-Host "Publicando $Version..."
dotnet publish TypeLibExporter_NET8/TypeLibExporter_NET8.csproj -c Release -r $rid --self-contained true /p:PublishSingleFile=true -o $outDir

if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path "$outDir/*" -DestinationPath $zipPath -Force

# Crear/editar release con gh
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
  throw 'gh CLI no está instalado o no está en PATH.'
}

$notesPath = "release-notes-$Version.md"
if (-not (Test-Path $notesPath)) {
  Set-Content -Path $notesPath -Value "# $Version`n`n- Cambios..." -Encoding UTF8
}

$tag = $Version
if (-not (git tag --list $tag)) {
  git tag -a $tag -m $tag
  git push origin $tag
}

$flags = @('--title', $Version, '--notes-file', $notesPath, '--verify-tag')
if ($Prerelease) { $flags += '--prerelease' }

if (-not (gh release view $tag -R . 2>$null)) {
  gh release create $tag @flags
} else {
  gh release edit $tag @flags
}

# Subir asset
gh release upload $tag $zipPath --clobber

Write-Host "Release $Version publicado con asset $zipPath" -ForegroundColor Green
