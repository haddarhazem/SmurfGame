using System;
using System.Drawing;
using System.IO;

class GenerateImages
{
    static void Main()
    {
        string root = @"C:\Users\User\Documents\c# et technologie .net\SmurfGame";
        
        try
        {
            // Generate smurf images
            Gen("papa smurf", Color.FromArgb(74, 150, 255), "P");  // Blue
            Gen("strong smurf", Color.FromArgb(200, 130, 50), "S"); // Brown
            Gen("lady smurf", Color.FromArgb(220, 100, 150), "L"); // Pink
            
            // Gargamel
            Single(Path.Combine(root, "images", "gargamel", "gargamel front.png"), 
                   Color.FromArgb(80, 80, 80), "G", Color.White);
            
            // Buffs
            Single(Path.Combine(root, "images", "buffs", "red buff.png"), 
                   Color.Red, "H", Color.White);
            Single(Path.Combine(root, "images", "buffs", "yellow buff.png"), 
                   Color.Yellow, "S", Color.Black);
            
            Console.WriteLine("✓ All images generated!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        void Gen(string name, Color bg, string text)
        {
            string dir = Path.Combine(root, "images", name);
            Single(Path.Combine(dir, $"{name} back.png"), bg, "↑", Color.White);
            Single(Path.Combine(dir, $"{name} face.png"), bg, "↓", Color.White);
            Single(Path.Combine(dir, $"{name} left.png"), bg, "←", Color.White);
            Single(Path.Combine(dir, $"{name} right.png"), bg, "→", Color.White);
            Console.WriteLine($"✓ {name}");
        }
        
        void Single(string path, Color bg, string text, Color fg)
        {
            using (var bmp = new Bitmap(32, 32))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(bg);
                    var font = new Font("Arial", 14, FontStyle.Bold);
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(text, font, new SolidBrush(fg), new RectangleF(0, 0, 32, 32), sf);
                }
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
