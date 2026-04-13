using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// Abstract base class for all Smurf types. Inherits from Creature.
    /// </summary>
    public abstract class Smurf : Creature
    {
        // Entity Framework properties (for database persistence)
        public int Id { get; set; }
        public int Level { get; set; } = 1;
        public bool IsInForest { get; set; } = false;

        protected Smurf(int startX, int startY, int speed, int maxHealth, MazeGenerator maze)
            : base(startX, startY, speed, maxHealth, maze)
        {
        }

        // Parameterless constructor for EF Core
        protected Smurf() : base(0, 0, 4, 100, null)
        {
        }
    }
}