namespace SmurfGame.BL.Entities
{
    /// <summary>
    /// RedBuff: Restores the target's Health.
    /// </summary>
    public class RedBuff : Buff
    {
        public const int HealthRestore = 30;

        public RedBuff(int x, int y, int duration = 300)
            : base(x, y, duration)
        {
        }

        /// <summary>
        /// Restores health to the creature.
        /// For LadySmurf, the effect is doubled.
        /// </summary>
        public override void ApplyTo(Creature target)
        {
            int heal = HealthRestore;

            // Double the effect for LadySmurf
            if (target is LadySmurf)
                heal = (int)(heal * LadySmurf.BuffMultiplier);

            target.Heal(heal);
        }
    }
}
