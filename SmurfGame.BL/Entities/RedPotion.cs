using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public class RedPotion : Item
    {
        [Range(1, 10)]
        public int BoostMultiplier { get; set; } = 2;
    }
}