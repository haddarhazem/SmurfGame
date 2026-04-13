using SmurfGame.DAL.Entities;

namespace SmurfGame.DAL.Repositories
{
    /// <summary>
    /// Repository for managing Score records in the database.
    /// </summary>
    public class ScoreRepository
    {
        private readonly SmurfDbContext _context;

        public ScoreRepository()
        {
            _context = new SmurfDbContext();
        }

        /// <summary>
        /// Saves a new score to the database.
        /// </summary>
        public void SaveScore(Score score)
        {
            if (score == null)
                throw new ArgumentNullException(nameof(score));

            _context.Scores.Add(score);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets the top N scores ordered by points (descending).
        /// </summary>
        public List<Score> GetTopScores(int count)
        {
            return _context.Scores
                .OrderByDescending(s => s.Points)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gets all scores ordered by PlayedAt (descending - most recent first).
        /// </summary>
        public List<Score> GetAllScores()
        {
            return _context.Scores
                .OrderByDescending(s => s.PlayedAt)
                .ToList();
        }
    }
}
