namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// LadySmurf: Can pick up buffs and their effect is doubled for her.
    /// </summary>
    public class LadySmurf : Smurf
    {
        public const double BuffMultiplier = 2.0;
        private int moveX = 0;
        private int moveY = 0;

        public LadySmurf(int startX, int startY, MazeGenerator maze)
            : base(startX, startY, speed: 5, maxHealth: 120, maze)
        {
        }

        // Parameterless constructor for Entity Framework
        protected LadySmurf() : base()
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
