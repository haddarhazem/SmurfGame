using System.ComponentModel.DataAnnotations;

namespace SmurfGame.BL.Entities
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}