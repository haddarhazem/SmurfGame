# Quick Start: Set Up Game Images

## Option 1: Use PowerShell Script (Fastest - Just Creates Folders)

**Step 1:** Open PowerShell and run:

```powershell
cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"
.\setup_images_folders.ps1
```

**Step 2:** Dismiss the "Missing Images" dialog when you run the game

**Step 3:** Game runs without visual sprites (but fully playable)

**Step 4:** Later, add your actual image files to each folder

---

## Option 2: Generate Placeholder Images (Recommended - See Colorful Sprites)

**Step 1:** Open PowerShell and run:

```powershell
cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"
csc GeneratePlaceholderImages.cs
.\GeneratePlaceholderImages.exe
```

**Step 2:** You'll see output like:
```
Creating placeholder images...

✓ Created folder: ...\images\papa smurf
✓ Created folder: ...\images\strong smurf
✓ Created folder: ...\images\lady smurf
✓ Created folder: ...\images\gargamel
✓ Created folder: ...\images\buffs
✓ Generated papa smurf images (back, face, left, right)
✓ Generated strong smurf images (back, face, left, right)
✓ Generated lady smurf images (back, face, left, right)
✓ Generated gargamel images (front)
✓ Generated buff images (red buff, yellow buff)

✓ All placeholder images created successfully!
```

**Step 3:** Run the game - **NO WARNING DIALOG** and you'll see colorful placeholder sprites!

**Step 4:** Later replace the .png files with your actual artwork

---

## What Gets Generated (Option 2)

### Characters
- **Papa Smurf:** Blue squares with arrows (↑↓←→)
- **Strong Smurf:** Brown squares with arrows (↑↓←→)
- **Lady Smurf:** Pink squares with arrows (↑↓←→)

### Enemy & Buffs
- **Gargamel:** Gray square with "G"
- **Red Buff:** Red square with "❤" (health)
- **Yellow Buff:** Yellow square with "⚡" (speed)

All are 32x32 PNG files with transparent backgrounds.

---

## Step-by-Step (Option 2)

### 1. Open PowerShell

Press `Win + X` and select **"Windows PowerShell (Admin)"** or **"Terminal (Admin)"**

### 2. Navigate to Project

```powershell
cd "C:\Users\User\Documents\c# et technologie .net\SmurfGame\"
```

### 3. Generate Images

```powershell
csc GeneratePlaceholderImages.cs
.\GeneratePlaceholderImages.exe
```

### 4. Build & Run Game

```powershell
dotnet build
dotnet run --project SmurfGame.WinForms
```

### 5. Play!

- **No warning dialog appears**
- **Game shows colorful placeholder sprites**
- **All game mechanics work**

---

## Verify Setup

Check that this folder structure exists:

```
SmurfGame/
└── images/
    ├── papa smurf/
    │   ├── papa smurf back.png ✓
    │   ├── papa smurf face.png ✓
    │   ├── papa smurf left.png ✓
    │   └── papa smurf right.png ✓
    ├── strong smurf/
    │   ├── strong smurf back.png ✓
    │   ├── strong smurf face.png ✓
    │   ├── strong smurf left.png ✓
    │   └── strong smurf right.png ✓
    ├── lady smurf/
    │   ├── lady smurf back.png ✓
    │   ├── lady smurf face.png ✓
    │   ├── lady smurf left.png ✓
    │   └── lady smurf right.png ✓
    ├── gargamel/
    │   └── gargamel front.png ✓
    └── buffs/
        ├── red buff.png ✓
        └── yellow buff.png ✓
```

---

## Troubleshooting

### PowerShell says "csc is not recognized"

Install .NET SDK:
```powershell
winget install Microsoft.dotnet-sdk.10
```

Then try again.

### "File already exists" error

Delete the `images` folder first:
```powershell
Remove-Item -Recurse -Force images
.\GeneratePlaceholderImages.exe
```

### Game still shows warning

Check PowerShell output for errors - file permissions issue? Try running PowerShell as Admin.

---

## Later: Replace with Real Images

Once you have proper sprite artwork:

1. Delete the placeholder .png files
2. Copy your actual sprites into each folder
3. Restart the game - instantly uses your new images!

No code changes needed - the image loader automatically picks up new files.

---

## Recommended: Option 2 (Placeholder Generation)

✅ **Pros:**
- See colorful sprites immediately
- Test all visuals while game runs
- Quick 30-second setup
- Easy to replace later

✅ **Cons:**
- Need to compile C# code
- Creates temporary .exe file

---

**Choose Option 2 and you'll have a fully visual game in seconds!** 🎮
