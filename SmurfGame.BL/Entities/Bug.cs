using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public abstract class Bug : Creature
    {
        [Range(1, 100)]
        public int AttackDamage { get; set; }
    }
}