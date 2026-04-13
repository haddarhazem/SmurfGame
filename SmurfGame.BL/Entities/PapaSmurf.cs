namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// PapaSmurf: The wise leader with the highest health.
    /// Player-controlled character.
    /// </summary>
    public class PapaSmurf : Smurf
    {
        private int moveX = 0;
        private int moveY = 0;

        public PapaSmurf(int startX, int startY, MazeGenerator maze)
            : base(startX, startY, speed: 4, maxHealth: 150, maze)
        {
        }

        // Parameterless constructor for Entity Framework
        protected PapaSmurf() : base()
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
