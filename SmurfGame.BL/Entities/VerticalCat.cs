using System;
using System.Collections.Generic;
using System.Text;

namespace SmurfGame.BL.Entities
{
    public class VerticalCat : Creature
    {
        public int Damage { get; set; } = 30; // Deals moderate damage
        public int Speed { get; set; } = 3;   // Moves vertically at speed 3 (slightly slower than player speed 5)
        public int Direction { get; set; } = 1; // 1 for down, -1 for up
    }
}