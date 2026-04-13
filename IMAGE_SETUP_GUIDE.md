# Setting Up Images Folder - SmurfGame

## Issue Fixed ✅

The application now includes **robust error handling** for missing image files. Instead of crashing, it will:
- Display a warning message explaining where to put images
- Continue running the game without sprites
- Allow you to play and test the game mechanics

---

## Image Folder Structure

You need to create the following folder structure in your project root:

```
SmurfGame/
└── images/
    ├── papa smurf/
    │   ├── papa smurf back.png
    │   ├── papa smurf face.png
    │   ├── papa smurf left.png
    │   └── papa smurf right.png
    │
    ├── strong smurf/
    │   ├── strong smurf back.png
    │   ├── strong smurf face.png
    │   ├── strong smurf left.png
    │   └── strong smurf right.png
    │
    ├── lady smurf/
    │   ├── lady smurf back.png
    │   ├── lady smurf face.png
    │   ├── lady smurf left.png
    │   └── lady smurf right.png
    │
    ├── gargamel/
    │   └── gargamel front.png
    │
    └── buffs/
        ├── red buff.png
        └── yellow buff.png
```

---

## How to Set Up Images

### Option 1: Copy Existing Images (If You Have Them)

1. **Navigate to project root:**
   ```powershell
   cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"
   ```

2. **Create images folder:**
   ```powershell
   mkdir images
   ```

3. **Copy your sprite sheets/images into the respective folders**

### Option 2: Create Placeholder Images (For Development)

If you don't have images yet, create simple placeholders:

**PowerShell Script to Create Placeholder Folders:**

```powershell
# Navigate to project root
cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"

# Create all necessary folders
$folders = @(
    "images/papa smurf",
    "images/strong smurf",
    "images/lady smurf",
    "images/gargamel",
    "images/buffs"
)

foreach ($folder in $folders) {
    if (!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Host "Created: $folder"
    }
}

Write-Host "All folders created!"
```

Run this in PowerShell:

```powershell
# Save as create_image_folders.ps1, then run:
.\create_image_folders.ps1
```

### Option 3: Configure Project to Copy Images on Build

Edit `SmurfGame.WinForms.csproj` to automatically copy images folder:

```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <!-- ... existing properties ... -->

  <!-- Add this ItemGroup to copy images on build -->
  <ItemGroup>
    <None Update="images/**/*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

---

## Image File Specifications

### Character Sprites (Papa/Strong/Lady Smurf)
- **Format:** PNG with transparency
- **Size:** 32x32 pixels recommended (can be any size)
- **Files needed per character:**
  - `[name] back.png` - Character facing away
  - `[name] face.png` - Character facing forward
  - `[name] left.png` - Character facing left
  - `[name] right.png` - Character facing right
- **Colors:** Use white (#FFFFFF) as the background for transparency

### Gargamel Enemy
- **File:** `gargamel front.png`
- **Format:** PNG with transparency
- **Size:** 32x32 pixels recommended
- **Colors:** Use white (#FFFFFF) as the background for transparency

### Buffs (Mushrooms)
- **Files:**
  - `red buff.png` - Red mushroom for health restoration
  - `yellow buff.png` - Yellow mushroom for speed boost
- **Format:** PNG with transparency
- **Size:** 16x16 pixels recommended
- **Colors:** Use white (#FFFFFF) as the background for transparency

---

## Test the Setup

### Step 1: Build the Application
```bash
dotnet build
```

### Step 2: Run the Application
```bash
dotnet run --project SmurfGame.WinForms
```

### Step 3: Check Results

**If images are found:**
- Application starts normally
- StartForm appears without warnings
- Game loads with character sprites visible

**If images are NOT found:**
- Warning dialog appears explaining the issue
- Application continues running
- Game is fully playable without sprites
- Check Visual Studio Debug output for file paths tried

---

## Troubleshooting

### "Warning: Image files not found"

This means the `images` folder wasn't found. Check:

1. **Folder location:**
   ```powershell
   # Should show images folder
   ls "C:\Users\User\Documents\c# et technologie .net\SmurfGame\" | grep images
   ```

2. **Folder structure:**
   ```powershell
   # Verify all subfolders exist
   ls "C:\Users\User\Documents\c# et technologie .net\SmurfGame\images"
   ```

3. **File names:**
   - Exact spelling matters: `papa smurf`, `strong smurf`, `lady smurf`
   - Spaces in folder names are important
   - File extensions must be `.png`

### Images Load but Show as Blank/White

This means:
- Files are found ✅
- But they may be completely white or have white background not matching code

**Fix:**
1. Ensure images have `Color.White` (#FFFFFF) as transparent background
2. Use image editor to verify the background color
3. Export as PNG with transparency preserved

### "File: ... not found" in Debug Output

Check `Output → Debug` window for the exact paths being searched. Common paths:
- `bin\Debug\net10.0-windows\images\`
- `bin\Release\net10.0-windows\images\`
- Project root: `images\`

---

## Getting Sprite Images

### Free Sprite Resources:

1. **OpenGameArt.org**
   - Free 2D game sprites
   - Search for "smurf" or create similar characters

2. **Itch.io**
   - Many free sprite packs
   - https://itch.io/game-assets/free/game-character

3. **Create Your Own**
   - Use Aseprite (paid) or Piskel (free)
   - Simple pixel art style works well
   - 32x32 pixels is standard for retro games

4. **AI Generated**
   - Use tools like Photoshop Generative Fill
   - OpenAI DALL-E (requires credits)

---

## Image Loading Logic (For Reference)

The application tries to load images from these locations (in order):

1. `./images/` (current directory)
2. `AppContext.BaseDirectory/images` (app execution folder)
3. `Directory.GetCurrentDirectory()/images` (working directory)
4. `AppDomain.CurrentDomain.BaseDirectory/images` (app domain base)

So as long as `images` folder exists in ANY of these locations, images will load.

---

## Quick Test Without Images

The game is **fully playable without images**:
- All game logic works (movement, collision, scoring)
- Sprites just won't display
- Perfect for testing mechanics

Just dismiss the warning dialog and play!

---

## Build Configuration

If you want images to auto-copy on build, edit `SmurfGame.WinForms.csproj`:

```xml
<ItemGroup>
  <None Update="images/**/*">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

Then rebuild. Images will automatically copy to output folder.

---

## Summary

✅ **Application now runs without crashing** even if images are missing
✅ **Clear error message** tells you exactly where to put images
✅ **Game is fully playable** without sprites
✅ **Flexible image loading** tries multiple paths
✅ **Null-safe code** handles missing images gracefully

You can now:
- Play and test the game immediately ▶️
- Add images anytime by creating the folder structure 🎨
- Continue development without waiting for artwork 🚀
