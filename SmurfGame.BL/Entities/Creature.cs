using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public abstract class Creature : Entity
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Health { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxHealth { get; set; }
    }
}