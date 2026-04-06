using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public class Smurf : Creature
    {
        [Range(1, 100)]
        public int Level { get; set; } = 1;
        public bool IsInForest { get; set; } = false;
        public int? BestTime { get; set; }
    }
}