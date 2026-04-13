namespace SmurfGame.BL
{
    /// <summary>
    /// Legacy Player class maintained for backward compatibility.
    /// New code should use Smurf subclasses (PapaSmurf, StrongSmurf, LadySmurf) instead.
    /// </summary>
    public class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Speed { get; set; } = 4;
        public int Size { get; set; } = 10; // Reduced from 14 for better maze traversal

        private readonly MazeGenerator maze;

        public Player(int startX, int startY, MazeGenerator maze)
        {
            X = startX;
            Y = startY;
            this.maze = maze;
        }

        public void SetPosition(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public void Move(int dx, int dy)
        {
            // Try X movement first
            if (dx != 0)
            {
                int newX = X + dx;
                // Check only center and one edge to allow squeezing through
                if (!maze.IsWall(newX + Size / 2, Y + Size / 2))
                {
                    X = newX;
                }
            }

            // Try Y movement separately
            if (dy != 0)
            {
                int newY = Y + dy;
                // Check only center point
                if (!maze.IsWall(X + Size / 2, newY + Size / 2))
                {
                    Y = newY;
                }
            }
        }
    }
}
