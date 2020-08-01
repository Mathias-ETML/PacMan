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
    public class PacMan
    {
        public void Spawn(Panel sender, EventArgs e)
        {
            Panel PlayerPacMan = new Panel()
            {
                Location = new Point(10, 10),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE)
            };

            sender.Controls.Add(PlayerPacMan);

            Graphics graphics = PlayerPacMan.CreateGraphics();

            Rectangle rectangle = new Rectangle(0, 0, PlayerPacMan.Width, PlayerPacMan.Height);

            graphics.DrawRectangle(new Pen(Color.Black, 10), rectangle);

            graphics.FillRectangle(new SolidBrush(Color.Yellow), rectangle);
        }

        public void Move()
        {

        }

        private bool CheckIfPacManCanMove()
        {
            return true;
        }

        public bool boolIsPacManDead { get; } = false;

        public void Die()
        {

        }

        public bool boolCanPacManEatGhost { get; set; } = false;

        public void Eat()
        {

        }
    }
}
