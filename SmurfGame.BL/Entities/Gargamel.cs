namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// Gargamel: The main antagonist who chases the player using simple movement.
    /// </summary>
    public class Gargamel : Villain
    {
        public Gargamel(int startX, int startY, MazeGenerator maze)
            : base(startX, startY, speed: 3, maxHealth: 100, damage: 30, maze)
        {
        }

        /// <summary>
        /// Moves one step closer to the target coordinates (player).
        /// </summary>
        public override void ChasePlayer(int targetX, int targetY)
        {
            int moveX = 0;
            int moveY = 0;

            // Determine horizontal movement
            if (X < targetX)
                moveX = Speed;
            else if (X > targetX)
                moveX = -Speed;

            // Determine vertical movement
            if (Y < targetY)
                moveY = Speed;
            else if (Y > targetY)
                moveY = -Speed;

            MoveWithCollision(moveX, moveY);
        }

        /// <summary>
        /// Gargamel moves by chasing; this should not be called directly, use ChasePlayer instead.
        /// </summary>
        public override void Move()
        {
            // Intentionally empty; use ChasePlayer instead
        }
    }
}
