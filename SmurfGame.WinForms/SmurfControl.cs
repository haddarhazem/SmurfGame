using SmurfGame.BL.Entities;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SmurfGame.WinForms
{
    public partial class SmurfControl : UserControl
    {
        // --- SMURF DATA ---
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Smurf CurrentSmurf { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Speed { get; set; } = 5;

        // --- ANIMATION FRAMES ---
        private Image[] idleFrames;
        private Image[] rightFrames;
        private Image[] leftFrames;
        private Image[] upFrames;
        private Image[] downFrames;

        // --- ANIMATION STATE ---
        private int currentIdleFrame = 0;
        private int currentRightFrame = 0;
        private int currentLeftFrame = 0;
        private int currentUpFrame = 0;
        private int currentDownFrame = 0;
        private bool isYawning = false;

        // --- TIMERS ---
        private System.Windows.Forms.Timer idleWaitTimer;
        private System.Windows.Forms.Timer animationTimer;

        // --- MOVEMENT TRACKING ---
        private List<PictureBox> obstacles = new List<PictureBox>();

        public SmurfControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Stretch;  // Ensure image stretches to fill control
            InitializeAnimationFrames();
            SetupTimers();
        }

        private void InitializeAnimationFrames()
        {
            idleFrames = new Image[]
            {
                Properties.Resources.standing0, Properties.Resources.standing1,
                Properties.Resources.standing2, Properties.Resources.standing3,
                Properties.Resources.standing4, Properties.Resources.standing5,
                Properties.Resources.standing6, Properties.Resources.standing7,
                Properties.Resources.standing8
            };

            rightFrames = new Image[] {
                Properties.Resources.right0,Properties.Resources.right1,
                Properties.Resources.right2,Properties.Resources.right3,
                Properties.Resources.right4,Properties.Resources.right5,
                Properties.Resources.right6,Properties.Resources.right7
            };

            leftFrames = new Image[] {
                Properties.Resources.left0,Properties.Resources.left1,
                Properties.Resources.left2,Properties.Resources.left3,
                Properties.Resources.left4,Properties.Resources.left5,
                Properties.Resources.left6
            };

            upFrames = new Image[] {
                Properties.Resources.up0,Properties.Resources.up1,
                Properties.Resources.up2,Properties.Resources.up3
            };

            downFrames = new Image[] {
                Properties.Resources.down0,Properties.Resources.down1,
                Properties.Resources.down2,Properties.Resources.down3
            };
        }

        private void SetupTimers()
        {
            // Idle wait timer (for yawning)
            idleWaitTimer = new System.Windows.Forms.Timer();
            idleWaitTimer.Interval = 10000; // 10 seconds
            idleWaitTimer.Tick += IdleWaitTimer_Tick;
            idleWaitTimer.Start();

            // Animation timer
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 150; // 150ms for smooth animation
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void IdleWaitTimer_Tick(object sender, EventArgs e)
        {
            idleWaitTimer.Stop();
            isYawning = true;
            currentIdleFrame = 5; // Jump to first yawn frame
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Update the image
            this.BackgroundImage = idleFrames[currentIdleFrame];
            currentIdleFrame++;

            if (isYawning == false)
            {
                // Normal breathing: frames 0-4
                if (currentIdleFrame > 4)
                {
                    currentIdleFrame = 0;
                }
            }
            else
            {
                // Yawning: frames 5-8
                if (currentIdleFrame > 8)
                {
                    isYawning = false;
                    currentIdleFrame = 0;
                    idleWaitTimer.Start(); // Restart the yawn countdown
                }
            }
        }

        // --- PUBLIC METHODS FOR MOVEMENT ---
        public void MoveUp()
        {
            if (this.Top > 0 && CanMove(this.Left, this.Top - Speed))
            {
                this.Top -= Speed;
                this.BackgroundImage = upFrames[currentUpFrame];
                currentUpFrame++;
                if (currentUpFrame >= upFrames.Length)
                {
                    currentUpFrame = 0;
                }
                StopIdleAnimation();
            }
        }

        public void MoveDown()
        {
            if (this.Bottom < this.Parent.ClientSize.Height && CanMove(this.Left, this.Top + Speed))
            {
                this.Top += Speed;
                this.BackgroundImage = downFrames[currentDownFrame];
                currentDownFrame++;
                if (currentDownFrame >= downFrames.Length)
                {
                    currentDownFrame = 0;
                }
                StopIdleAnimation();
            }
        }

        public void MoveLeft()
        {
            if (this.Left > 0 && CanMove(this.Left - Speed, this.Top))
            {
                this.Left -= Speed;
                this.BackgroundImage = leftFrames[currentLeftFrame];
                currentLeftFrame++;
                if (currentLeftFrame >= leftFrames.Length)
                {
                    currentLeftFrame = 0;
                }
                StopIdleAnimation();
            }
        }

        public void MoveRight()
        {
            if (this.Right < this.Parent.ClientSize.Width && CanMove(this.Left + Speed, this.Top))
            {
                this.Left += Speed;
                this.BackgroundImage = rightFrames[currentRightFrame];
                currentRightFrame++;
                if (currentRightFrame >= rightFrames.Length)
                {
                    currentRightFrame = 0;
                }
                StopIdleAnimation();
            }
        }

        private void StopIdleAnimation()
        {
            idleWaitTimer.Stop();
            animationTimer.Stop();
            isYawning = false;
            currentIdleFrame = 0;
        }

        public void ResumeAnimation()
        {
            currentIdleFrame = 0;
            idleWaitTimer.Start();
            animationTimer.Start();
        }

        public void SetObstacles(List<PictureBox> obstacleList)
        {
            obstacles = obstacleList;
        }

        private bool CanMove(int futureX, int futureY)
        {
            Rectangle ghostSmurf = new Rectangle(futureX, futureY, this.Width, this.Height);

            foreach (PictureBox wall in obstacles)
            {
                if (ghostSmurf.IntersectsWith(wall.Bounds))
                {
                    return false;
                }
            }

            return true;
        }

        public Point GetPosition()
        {
            return new Point(this.Left, this.Top);
        }

        public void SetPosition(int x, int y)
        {
            this.Left = x;
            this.Top = y;
        }

        public Rectangle GetBounds()
        {
            return this.Bounds;
        }

        public void Dispose()
        {
            if (idleWaitTimer != null)
            {
                idleWaitTimer.Stop();
                idleWaitTimer.Dispose();
            }

            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
            }

            base.Dispose();
        }
    }
}
