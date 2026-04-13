namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// YellowBuff: Temporarily increases the target's Speed.
    /// </summary>
    public class YellowBuff : Buff
    {
        public const int SpeedBoost = 4;

        public YellowBuff(int x, int y, int duration = 300)
            : base(x, y, duration)
        {
        }

        /// <summary>
        /// Applies a temporary speed boost to the creature.
        /// For LadySmurf, the effect is doubled.
        /// </summary>
        public override void ApplyTo(Creature target)
        {
            int boost = SpeedBoost;

            // Double the effect for LadySmurf
            if (target is LadySmurf)
                boost = (int)(boost * LadySmurf.BuffMultiplier);

            target.Speed += boost;
        }
    }
}
