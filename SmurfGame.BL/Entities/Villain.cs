namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// Abstract base class for all villains/enemies. Inherits from Creature.
    /// </summary>
    public abstract class Villain : Creature
    {
        public int Damage { get; set; }

        protected Villain(int startX, int startY, int speed, int maxHealth, int damage, MazeGenerator maze)
            : base(startX, startY, speed, maxHealth, maze)
        {
            Damage = damage;
        }

        /// <summary>
        /// Abstract method for chasing the player.
        /// </summary>
        public abstract void ChasePlayer(int targetX, int targetY);
    }
}
