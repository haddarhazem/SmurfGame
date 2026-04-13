namespace SmurfGame.BL.Controllers
{
    /// <summary>
    /// Indépendant contrôleur de joueur qui gère le mouvement et les contrôles
    /// Séparé de la logique UI du formulaire pour meilleure architecture
    /// </summary>
    public class PlayerController
    {
        private int _x;
        private int _y;
        private int _speed;
        private int _width;
        private int _height;

        /// <summary>
        /// Direction du joueur
        /// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Idle
        }

        /// <summary>
        /// Direction actuelle du joueur
        /// </summary>
        public Direction CurrentDirection { get; set; } = Direction.Idle;

        /// <summary>
        /// Position X du joueur
        /// </summary>
        public int X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Position Y du joueur
        /// </summary>
        public int Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Vitesse de déplacement du joueur
        /// </summary>
        public int Speed
        {
            get => _speed;
            set => _speed = value;
        }

        /// <summary>
        /// Largeur du joueur (pour les collisions)
        /// </summary>
        public int Width
        {
            get => _width;
            set => _width = value;
        }

        /// <summary>
        /// Hauteur du joueur (pour les collisions)
        /// </summary>
        public int Height
        {
            get => _height;
            set => _height = value;
        }

        /// <summary>
        /// Constructeur du contrôleur de joueur
        /// </summary>
        public PlayerController(int startX, int startY, int speed, int width, int height)
        {
            _x = startX;
            _y = startY;
            _speed = speed;
            _width = width;
            _height = height;
            CurrentDirection = Direction.Idle;
        }

        /// <summary>
        /// Déplace le joueur vers le haut
        /// </summary>
        /// <param name="maxY">Hauteur maximum de l'écran</param>
        /// <param name="canMove">Fonction de vérification des collisions</param>
        /// <returns>true si le mouvement a réussi, false sinon</returns>
        public bool MoveUp(int maxY, Func<int, int, bool> canMove)
        {
            if (_y > 0 && canMove(_x, _y - _speed))
            {
                _y -= _speed;
                CurrentDirection = Direction.Up;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Déplace le joueur vers le bas
        /// </summary>
        /// <param name="maxY">Hauteur maximum de l'écran</param>
        /// <param name="canMove">Fonction de vérification des collisions</param>
        /// <returns>true si le mouvement a réussi, false sinon</returns>
        public bool MoveDown(int maxY, Func<int, int, bool> canMove)
        {
            if (_y + _height < maxY && canMove(_x, _y + _speed))
            {
                _y += _speed;
                CurrentDirection = Direction.Down;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Déplace le joueur vers la gauche
        /// </summary>
        /// <param name="maxX">Largeur maximum de l'écran</param>
        /// <param name="canMove">Fonction de vérification des collisions</param>
        /// <returns>true si le mouvement a réussi, false sinon</returns>
        public bool MoveLeft(int maxX, Func<int, int, bool> canMove)
        {
            if (_x > 0 && canMove(_x - _speed, _y))
            {
                _x -= _speed;
                CurrentDirection = Direction.Left;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Déplace le joueur vers la droite
        /// </summary>
        /// <param name="maxX">Largeur maximum de l'écran</param>
        /// <param name="canMove">Fonction de vérification des collisions</param>
        /// <returns>true si le mouvement a réussi, false sinon</returns>
        public bool MoveRight(int maxX, Func<int, int, bool> canMove)
        {
            if (_x + _width < maxX && canMove(_x + _speed, _y))
            {
                _x += _speed;
                CurrentDirection = Direction.Right;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Applique un buff de vitesse
        /// </summary>
        /// <param name="boostAmount">Montant du boost de vitesse</param>
        public void ApplySpeedBuff(int boostAmount)
        {
            _speed += boostAmount;
        }

        /// <summary>
        /// Retire un buff de vitesse
        /// </summary>
        /// <param name="boostAmount">Montant du buff à retirer</param>
        public void RemoveSpeedBuff(int boostAmount)
        {
            _speed -= boostAmount;
        }

        /// <summary>
        /// Réinitialise la position du joueur
        /// </summary>
        /// <param name="startX">Position X initiale</param>
        /// <param name="startY">Position Y initiale</param>
        public void ResetPosition(int startX, int startY)
        {
            _x = startX;
            _y = startY;
            CurrentDirection = Direction.Idle;
        }

        /// <summary>
        /// Réinitialise la vitesse à la valeur par défaut
        /// </summary>
        /// <param name="defaultSpeed">Vitesse par défaut</param>
        public void ResetSpeed(int defaultSpeed)
        {
            _speed = defaultSpeed;
        }

        /// <summary>
        /// Obtient les informations actuelles du joueur
        /// </summary>
        public override string ToString()
        {
            return $"PlayerController - Position: ({_x}, {_y}), Speed: {_speed}, Direction: {CurrentDirection}";
        }
    }
}
