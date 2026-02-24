using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public abstract class Item : Entity
    {
        [Range(1, 500)]
        public int HealthBoost { get; set; }
        public bool IsConsumed { get; set; } = false;
    }
}