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
        public Panel Body { get; set; }

        public PacMan()
        {
            Body = new Panel()
            {
                Location = new Point(30, 30),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE)
            };

            Graphics graphics = Body.CreateGraphics();

            // todo : graphic design for pacman

            Rectangle rectangle = new Rectangle(0, 0, Body.Width, Body.Height);

            graphics.DrawRectangle(new Pen(Color.Black, 10), rectangle);

            graphics.FillRectangle(new SolidBrush(Color.Yellow), rectangle);
        }

        public void StartPacManAnimation()
        {
            if (timer_PlayerAnimation == null)
            {
                timer_PlayerAnimation = new Timer()
                {
                    Interval = G_BYTETIMEBETWENGAMETICK,
                    Enabled = true
                };

                timer_PlayerAnimation.Tick += PacManAnimation;
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
        public int intPacManPosX { get; set; } = 0;
        public int intPacManPosY { get; set; } = 0;

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

        public bool boolIsPacManDead { get; } = false;

        public void Die()
        {
            if (Body != null)
            {
                StopPacManAnimation();
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
