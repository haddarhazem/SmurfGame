namespace SmurfGame.DAL.Entities
{
    /// <summary>
    /// Represents a player's game score/session record.
    /// </summary>
    public class Score
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string SmurfType { get; set; } = string.Empty;
        public int Points { get; set; } // Survival time in seconds
        public DateTime PlayedAt { get; set; }
    }
}
