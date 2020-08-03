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
            g_pacMans = new PacMan[g_byteNumberOfPlayer];

            // init the array for the ghost
            g_ghosts = new Ghost[G_NUMBEROFGHOST];

            // init the player/s packman/s
            for (; g_byteNumberOfPacMan < g_byteNumberOfPlayer; g_byteNumberOfPacMan++)
            {
                g_pacMans[g_byteNumberOfPacMan] = new PacMan();
            }

            // init the 4 ghost
            for (; g_byteNumberOfSpawnedGhost < G_NUMBEROFGHOST; g_byteNumberOfSpawnedGhost++)
            {
                g_ghosts[g_byteNumberOfSpawnedGhost] = new Ghost();
            }
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            
        }
    }
}
