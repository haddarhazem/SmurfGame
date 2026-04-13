namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// StrongSmurf: The strongest with highest speed and can deal damage back to Gargamel on collision.
    /// </summary>
    public class StrongSmurf : Smurf
    {
        public int CounterDamage { get; set; } = 20;
        private int moveX = 0;
        private int moveY = 0;

        public StrongSmurf(int startX, int startY, MazeGenerator maze)
            : base(startX, startY, speed: 6, maxHealth: 100, maze)
        {
        }

        // Parameterless constructor for Entity Framework
        protected StrongSmurf() : base()
        {
        }

        /// <summary>
        /// Sets the current movement direction based on input.
        /// </summary>
        public void SetMovementInput(int dx, int dy)
        {
            moveX = dx;
            moveY = dy;
        }

        /// <summary>
        /// Moves based on current input direction.
        /// </summary>
        public override void Move()
        {
            if (moveX != 0 || moveY != 0)
            {
                MoveWithCollision(moveX * Speed, moveY * Speed);
            }
        }
    }
}
