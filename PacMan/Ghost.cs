﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class Ghost
    {
        public Panel Body { get; set; }

        public Ghost()
        {
            Body = new Panel()
            {
                Location = new Point(30, 30),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE)
            };
        }

        // int x etc.. where to move
        public int intGhostPosX { get; set; } = 0;
        public int intGhostPosY { get; set; } = 0;

        public int intGhostMovementX { get; set; } = 0;
        public int intGhostMovementY { get; set; } = 0;

        public void Move()
        {
            if (Body != null)
            {

            }
        }

        public int intWhereGhostNeedToMoveX { get; set; } = 0;
        public int intWhereGhostNeedToMoveY { get; set; } = 0;

        public void ChangeGhostDirection()
        {
            if (Body != null)
            {

            }
        }
    }
}
