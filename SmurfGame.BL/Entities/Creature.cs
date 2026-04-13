using System.ComponentModel.DataAnnotations.Schema;

namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// Abstract base class for all moving creatures in the game (Smurfs and Villains).
    /// </summary>
    public abstract class Creature
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Size { get; set; } = 14; // hitbox size

        [NotMapped]
        protected readonly MazeGenerator? Maze;

        protected Creature(int startX, int startY, int speed, int maxHealth, MazeGenerator? maze)
        {
            X = startX;
            Y = startY;
            Speed = speed;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Maze = maze;
        }

        // Parameterless constructor for serialization/EF Core
        protected Creature()
        {
        }

        /// <summary>
        /// Abstract method for movement logic, to be implemented by subclasses.
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Moves the creature in the specified direction with collision detection.
        /// </summary>
        protected void MoveWithCollision(int dx, int dy)
        {
            if (Maze == null) return; // Skip if no maze reference

            int newX = X + dx;
            int newY = Y + dy;

            // Check X movement independently for sliding along walls
            bool okX = !Maze.IsWall(newX, Y) &&
                       !Maze.IsWall(newX + Size, Y) &&
                       !Maze.IsWall(newX, Y + Size) &&
                       !Maze.IsWall(newX + Size, Y + Size);

            if (okX)
            {
                X = newX; // Commit X movement if no wall collision
            }

            // Check Y movement
            bool okY = !Maze.IsWall(X, newY) &&
                       !Maze.IsWall(X + Size, newY) &&
                       !Maze.IsWall(X, newY + Size) &&
                       !Maze.IsWall(X + Size, newY + Size);

            if (okY)
            {
                Y = newY; // Commit Y movement if no wall collision
            }
        }

        /// <summary>
        /// Takes damage and clamps health to 0.
        /// </summary>
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        /// <summary>
        /// Restores health up to MaxHealth.
        /// </summary>
        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        /// <summary>
        /// Checks if two creatures are touching (collision detection).
        /// </summary>
        public bool IsTouching(Creature other)
        {
            return Math.Abs(X - other.X) < Size && Math.Abs(Y - other.Y) < Size;
        }
    }
}