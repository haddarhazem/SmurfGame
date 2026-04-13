using System;
using System.Drawing;
using System.IO;

class PlaceholderImageGenerator
{
    static void Main()
    {
        string projectRoot = @"C:\Users\User\Documents\c# et technologie .net\SmurfGame";
        
        try
        {
            Console.WriteLine("Creating placeholder images...\n");

            // Create folder structure
            CreateFolders(projectRoot);

            // Generate smurf images
            GenerateSmurfImages(projectRoot, "papa smurf", Color.FromArgb(74, 150, 255)); // Blue
            GenerateSmurfImages(projectRoot, "strong smurf", Color.FromArgb(200, 130, 50)); // Brown
            GenerateSmurfImages(projectRoot, "lady smurf", Color.FromArgb(220, 100, 150)); // Pink

            // Generate Gargamel image
            GenerateImage(Path.Combine(projectRoot, "images", "gargamel", "gargamel front.png"), 
                         Color.FromArgb(80, 80, 80), "G", Color.White);

            // Generate buff images
            GenerateImage(Path.Combine(projectRoot, "images", "buffs", "red buff.png"), 
                         Color.Red, "❤", Color.White);
            GenerateImage(Path.Combine(projectRoot, "images", "buffs", "yellow buff.png"), 
                         Color.Yellow, "⚡", Color.Black);

            Console.WriteLine("\n✓ All placeholder images created successfully!");
            Console.WriteLine("\nNow run the game and it should display the placeholder sprites.");
            Console.WriteLine("Later, replace these .png files with your actual artwork.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void CreateFolders(string projectRoot)
    {
        string[] folders = new[]
        {
            Path.Combine(projectRoot, "images", "papa smurf"),
            Path.Combine(projectRoot, "images", "strong smurf"),
            Path.Combine(projectRoot, "images", "lady smurf"),
            Path.Combine(projectRoot, "images", "gargamel"),
            Path.Combine(projectRoot, "images", "buffs")
        };

        foreach (var folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Console.WriteLine($"✓ Created folder: {folder}");
            }
        }
    }

    static void GenerateSmurfImages(string projectRoot, string smurfType, Color baseColor)
    {
        string folderPath = Path.Combine(projectRoot, "images", smurfType);

        GenerateImage(Path.Combine(folderPath, $"{smurfType} back.png"), baseColor, "↑", Color.White);
        GenerateImage(Path.Combine(folderPath, $"{smurfType} face.png"), baseColor, "↓", Color.White);
        GenerateImage(Path.Combine(folderPath, $"{smurfType} left.png"), baseColor, "←", Color.White);
        GenerateImage(Path.Combine(folderPath, $"{smurfType} right.png"), baseColor, "→", Color.White);

        Console.WriteLine($"✓ Generated {smurfType} images (back, face, left, right)");
    }

    static void GenerateImage(string filePath, Color bgColor, string symbol, Color textColor)
    {
        using (Bitmap bmp = new Bitmap(32, 32))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fill background
                g.Clear(bgColor);

                // Draw symbol
                Font font = new Font("Arial", 16, FontStyle.Bold);
                StringFormat sf = new StringFormat 
                { 
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center 
                };

                g.DrawString(symbol, font, new SolidBrush(textColor), 
                           new RectangleF(0, 0, 32, 32), sf);

                font.Dispose();
            }

            // Save PNG
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
