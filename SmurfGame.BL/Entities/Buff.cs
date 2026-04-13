namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// Abstract base class for all buffs/power-ups. Does NOT inherit from Creature.
    /// </summary>
    public abstract class Buff
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Duration { get; set; } // in ticks/frames
        public int Size { get; set; } = 12; // collision size

        protected Buff(int x, int y, int duration)
        {
            X = x;
            Y = y;
            Duration = duration;
        }

        /// <summary>
        /// Applies the buff effect to the target creature.
        /// </summary>
        public abstract void ApplyTo(Creature target);

        /// <summary>
        /// Checks if the buff is touching a creature.
        /// </summary>
        public bool IsTouching(Creature creature)
        {
            return Math.Abs(X - creature.X) < Size + creature.Size &&
                   Math.Abs(Y - creature.Y) < Size + creature.Size;
        }
    }
}
