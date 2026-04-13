# SmurfGame Images Folder Setup Script
# Run this in PowerShell from the project root directory

# Navigate to project root
cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"

# Create all image folder structure
$folders = @(
    "images\papa smurf",
    "images\strong smurf",
    "images\lady smurf",
    "images\gargamel",
    "images\buffs"
)

Write-Host "Creating image folder structure..." -ForegroundColor Green

foreach ($folder in $folders) {
    if (!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Host "✓ Created: $folder" -ForegroundColor Green
    } else {
        Write-Host "✓ Already exists: $folder" -ForegroundColor Cyan
    }
}

Write-Host "`nFolder structure created successfully!" -ForegroundColor Green
Write-Host "Now add your .png image files to each folder:" -ForegroundColor Yellow
Write-Host "  - papa smurf: back.png, face.png, left.png, right.png"
Write-Host "  - strong smurf: back.png, face.png, left.png, right.png"
Write-Host "  - lady smurf: back.png, face.png, left.png, right.png"
Write-Host "  - gargamel: front.png"
Write-Host "  - buffs: red buff.png, yellow buff.png"
