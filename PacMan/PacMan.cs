using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class PacMan
    {
        private Timer timer_PlayerAnimation;
        public Panel Body { get; }
        private Rectangle mouth { get; set; }

        public PacMan(int x, int y)
        {
            Body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE),
                BackColor = Color.Black
            };

            Body.Paint += Body_Paint;
        }

        private void Body_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = Body.CreateGraphics();

            // todo : graphic design for pacman

            Rectangle rectangle = new Rectangle(3, 3, Body.Width - 7, Body.Height - 7);

            graphics.DrawEllipse(new Pen(Color.Yellow, 5), rectangle);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), rectangle);
        }

        public void StartPacManAnimation()
        {
            if (timer_PlayerAnimation == null)
            {
                timer_PlayerAnimation = new Timer()
                {
                    Interval = G_BYTETIMEBETWENGAMETICK,
                };

                timer_PlayerAnimation.Tick += PacManAnimation;

                timer_PlayerAnimation.Start();
            }
        }

        public void StopPacManAnimation()
        {
            if (timer_PlayerAnimation != null)
            {
                timer_PlayerAnimation.Stop();
                timer_PlayerAnimation.Dispose();
            }
        }

        private bool boolIsPacManMouthOpen { get; set; } = false;

        private void PacManAnimation(object sender, EventArgs e)
        {
            if (Body != null)
            {
                
            }
        }

        // int x etc.. where to move
        public int intPacManPosX
        {
            get => Body.Location.X;
            set
            {
                if (Body != null)
                {
                    Body.Location = new Point(value, Body.Location.Y);
                }

            }
        }

        public int intPacManPosY
        {
            get => Body.Location.Y;
            set
            {
                if (Body != null)
                {
                    Body.Location = new Point(value, Body.Location.Y);
                }

            }
        }

        public int intPacManMovementX { get; set; } = 0;
        public int intPacManMovementY { get; set; } = 0;

        public void Move()
        {
            if (Body != null)
            {

            }
        }

        private bool CheckIfPacManCanMove()
        {
            return true;
        }

        public bool boolIsPacManDead { get; set; } = false;

        public void Die()
        {
            if (Body != null && this != null)
            {
                StopPacManAnimation();
                Body.Dispose();
                ((IDisposable)this).Dispose();
            }
            else if (this != null)
            {
                ((IDisposable)this).Dispose();
            }
        }

        public bool boolCanPacManEatGhost { get; set; } = false;

        public void Eat()
        {

        }

        public ulong g_U64PlayerScore { get; set; } = 0;

        private void AddPlayerPoints()
        {

        }

        public char OnWichCaseIsPackMan()
        {
            return '\0';
        }
    }
}
