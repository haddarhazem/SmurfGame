using System;
using System.Linq;

namespace SmurfGame.BL
{
    public class MazeGenerator
    {
        public int Cols { get; }
        public int Rows { get; }
        public int TileW { get; }
        public int TileH { get; }

        private readonly bool[,] wallS; // south wall of cell [r,c]
        private readonly bool[,] wallE; // east wall of cell [r,c]
        private readonly bool[,] visited;
        private readonly Random rng = new Random(42);

        public MazeGenerator(int cols, int rows, int tileW, int tileH)
        {
            Cols = cols; Rows = rows; TileW = tileW; TileH = tileH;
            wallS = new bool[rows, cols];
            wallE = new bool[rows, cols];
            visited = new bool[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                { wallS[r, c] = true; wallE[r, c] = true; }

            Carve(0, 0);
        }

        private void Carve(int r, int c)
        {
            visited[r, c] = true;
            var dirs = new[] { 0, 1, 2, 3 }.OrderBy(_ => rng.Next()).ToArray();

            foreach (int d in dirs)
            {
                int nr = r + (d == 1 ? 1 : d == 0 ? -1 : 0);
                int nc = c + (d == 3 ? -1 : d == 2 ? 1 : 0);

                if (nr < 0 || nr >= Rows || nc < 0 || nc >= Cols) continue;
                if (visited[nr, nc]) continue;

                if (d == 1) wallS[r, c] = false; // going south
                if (d == 0) wallS[nr, c] = false; // going north
                if (d == 2) wallE[r, c] = false; // going east
                if (d == 3) wallE[r, nc] = false; // going west

                Carve(nr, nc);
            }
        }

        public bool IsWall(int pixelX, int pixelY)
        {
            // Global boundary
            if (pixelX < 0 || pixelY < 0) return true;
            if (pixelX >= Cols * TileW || pixelY >= Rows * TileH) return true;

            int c = pixelX / TileW;
            int r = pixelY / TileH;

            // Clamp to valid cell range
            if (r >= Rows || c >= Cols) return true;

            int localX = pixelX % TileW;
            int localY = pixelY % TileH;

            // W must match EXACTLY how thick walls are drawn on screen
            // If you draw walls with a pen of thickness 6, keep W = 6
            const int W = 6;

            // West wall
            if (localX < W && (c == 0 || wallE[r, c - 1])) return true;

            // East wall
            if (localX > TileW - W && (c == Cols - 1 || wallE[r, c])) return true;

            // North wall
            if (localY < W && (r == 0 || wallS[r - 1, c])) return true;

            // South wall
            if (localY > TileH - W && (r == Rows - 1 || wallS[r, c])) return true;

            return false;
        }

        public bool IsHorzWall(int r, int c) => wallS[r, c];
        public bool IsVertWall(int r, int c) => wallE[r, c];
    }
}