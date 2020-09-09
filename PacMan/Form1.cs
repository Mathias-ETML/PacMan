﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class frm_FormPrincipal : Form
    {
        public Timer timer;

        public frm_FormPrincipal()
        {
            InitializeComponent();
            OnStart();
        }

        private void OnStart()
        {
            // init the OnUpdate
            timer = new Timer()
            {
                Interval = G_BYTETIMEBETWENGAMETICK,
                Enabled = true
            };

            timer.Tick += OnUpdate;

            pan_PanMap.Paint += pan_PanGame_Paint;

            // init the array for the pacman's
            g_tab_pacMans = new PacMan[g_byteNumberOfPlayer];

            // init the array for the ghost
            g_tab_ghosts = new Ghost[G_NUMBEROFGHOST];
            
            // init the player/s packman/s
            for (; g_byteNumberOfPacMan < g_byteNumberOfPlayer; g_byteNumberOfPacMan++)
            {
                //g_tab_pacMans[g_byteNumberOfPacMan] = new PacMan(680, 360);
                g_tab_pacMans[g_byteNumberOfPacMan] = new PacMan(9 * G_BYTESIZEOFSQUARE, 15 * G_BYTESIZEOFSQUARE);
                pan_PanMap.Controls.Add(g_tab_pacMans[g_byteNumberOfPacMan].Body);
            }
        }

        /// <summary>
        /// This is the main loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdate(object sender, EventArgs e)
        {
            g_tab_pacMans[0].Move();

            pan_PanMap.Paint += UpdateMap;

            label1.Text = g_tab_pacMans[0].Body.Location.ToString();
        }

        private void UpdateMap(object sender, PaintEventArgs e)
        {
            g_tab_pacMans[0].UpdateMap(e.Graphics);

            ((Panel)sender).Paint -= UpdateMap;
        }

        private void pan_PanGame_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            for (int y = 0; y < Map.MapHeight; y++)
            {
                for (int x = 0; x < Map.MapWidth; x++)
                {
                    Map.DrawMapRectangle(graphics, Map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);

                    Map.FoodMap.tab_foods[y, x] = new Food(graphics, (Food.FoodMeaning)Map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);
                }
            }

            ((Panel)sender).Paint -= pan_PanGame_Paint;
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (Mouth.Position.North != Mouth.MouthDirection)
                    {
                        g_tab_pacMans[0].SetPacManDeplacement(0, -PacMan.SpeedOfPacMan);
                        g_tab_pacMans[0].RotatePacMan_body(Mouth.Position.North);
                    }
                    break;

                case Keys.Right:
                    if (Mouth.Position.South != Mouth.MouthDirection)
                    {
                        g_tab_pacMans[0].SetPacManDeplacement(PacMan.SpeedOfPacMan, 0);
                        g_tab_pacMans[0].RotatePacMan_body(Mouth.Position.South);
                    }
                    break;

                case Keys.Down:
                    if (Mouth.Position.East != Mouth.MouthDirection)
                    {
                        g_tab_pacMans[0].SetPacManDeplacement(0, PacMan.SpeedOfPacMan);
                        g_tab_pacMans[0].RotatePacMan_body(Mouth.Position.East);
                    }
                    break;

                case Keys.Left:
                    if (Mouth.Position.West != Mouth.MouthDirection)
                    {
                        g_tab_pacMans[0].SetPacManDeplacement(-PacMan.SpeedOfPacMan, 0);
                        g_tab_pacMans[0].RotatePacMan_body(Mouth.Position.West);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
