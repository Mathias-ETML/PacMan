using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Map map;
        public Map.FoodMap foodMap;

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

            // init the array for the pacman's
            g_tab_pacMans = new PacMan[g_byteNumberOfPlayer];

            // init the array for the ghost
            g_tab_ghosts = new Ghost[G_NUMBEROFGHOST];
            
            // init the player/s packman/s
            for (; g_byteNumberOfPacMan < g_byteNumberOfPlayer; g_byteNumberOfPacMan++)
            {
                g_tab_pacMans[g_byteNumberOfPacMan] = new PacMan(9 * G_BYTESIZEOFSQUARE, 15 * G_BYTESIZEOFSQUARE);
                pan_PanGame.Controls.Add(g_tab_pacMans[g_byteNumberOfPacMan].Body);
            }

            /*
            // init the 4 ghost
            for (; g_byteNumberOfSpawnedGhost < G_NUMBEROFGHOST; g_byteNumberOfSpawnedGhost++)
            {
                g_tab_ghosts[g_byteNumberOfSpawnedGhost] = new Ghost(0, 0);
                pan_PanGame.Controls.Add(g_tab_ghosts[g_byteNumberOfSpawnedGhost].Body);
            }
            */
            
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            g_tab_pacMans[0].Move();
        }

        private void pan_PanGame_Paint(object sender, PaintEventArgs e)
        {
            if (map == null && foodMap == null)
            {
                map = new Map();
                foodMap = new Map.FoodMap();

                Graphics graphics = pan_PanGame.CreateGraphics();

                for (int x = 0; x < Map.MapHeight; x++)
                {
                    for (int y = 0; y < Map.MapWidth; y++)
                    {
                        map.DrawMapRectangle(graphics, map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);

                        foodMap.tab_foods[y, x] = new Food(graphics, (Food.FoodMeaning)map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);
                    }
                }
            }
        }
    }
}
