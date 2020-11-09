using System;
using System.Drawing;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class frm_FormPrincipal : Form
    {
        #region Atrributs
        /// <summary>
        /// Attributs
        /// </summary>
        private Timer _timer;
        private Map _map;
        #endregion Attributs

        #region Proprieties
        /// <summary>
        /// Proprieties
        /// </summary>
        public Map Map { get => _map; }
        #endregion Proprieties

        #region Constructor
        /// <summary>
        /// Custom constructor
        /// </summary>
        public frm_FormPrincipal()
        {
            InitializeComponent();
            OnStart();
        }
        #endregion Construcotr

        #region On start
        private void OnStart()
        {
            // create the map of the game
            this._map = new Map();

            // init the OnUpdate
            _timer = new Timer()
            {
                Interval = G_BYTETIMEBETWENGAMETICK,
                Enabled = true
            };
            _timer.Tick += OnUpdate;

            // init the first painting of the map
            this.pan_PanMap.Paint += FoodMapDisposition;

            // init the array for the pacman's
            G_pacMans = new PacMan[G_numberOfPlayer];

            // init the array for the ghost
            G_ghosts = new Ghost[G_NUMBEROFGHOST];

            // init the player/s packman/s
            for (; G_numberOfPacMan < G_numberOfPlayer; G_numberOfPacMan++)
            {
                //g_tab_pacMans[g_byteNumberOfPacMan] = new PacMan(680, 360);
                G_pacMans[G_numberOfPacMan] = new PacMan(9 * G_BYTESIZEOFSQUARE, 15 * G_BYTESIZEOFSQUARE, _map);
                pan_PanMap.Controls.Add(G_pacMans[G_numberOfPacMan].Body);
            }
        }
        #endregion OnStart

        #region main loop
        /// <summary>
        /// This is the main loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdate(object sender, EventArgs e)
        {
            if (G_pacMans[0].Move())
            {
                pan_PanMap.Paint += UpdateMapTeleportation;
            }

            pan_PanMap.Paint += UpdateMap;

            label1.Text = G_pacMans[0].Body.Location.ToString();
        }
        #endregion main loop

        #region Map update
        private void UpdateMap(object sender, PaintEventArgs e)
        {
            G_pacMans[0].UpdateMap(sender, e);

            ((Panel)sender).Paint -= UpdateMap;
        }

        private void UpdateMapTeleportation(object sender, PaintEventArgs e)
        {
            G_pacMans[0].UpdateTeleportation(sender, e);

            ((Panel)sender).Paint -= UpdateMapTeleportation;
        }
        #endregion Map update

        #region food map application
        private void FoodMapDisposition(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            for (int y = 0; y < Map.MapHeight; y++)
            {
                for (int x = 0; x < Map.MapWidth; x++)
                {
                    Map.DrawMapRectangle(graphics, Map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);

                    _map.FoodsMap[y, x] = new Food(graphics, (Food.FoodMeaning)Map.GameMap[y, x], x * G_BYTESIZEOFSQUARE, y * G_BYTESIZEOFSQUARE);
                }
            }

            ((Panel)sender).Paint -= FoodMapDisposition;
        }
        #endregion food map application

        #region user input
        /// <summary>
        /// Get what key we pressed
        /// </summary>
        /// <param name="sender">the panel</param>
        /// <param name="e">informations</param>
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (PacMan.Mouth.Position.North != G_pacMans[0].ActualMouthDirection)
                    {
                        G_pacMans[0].SetPacManDeplacement(0, -PacMan.SpeedOfPacMan);
                        G_pacMans[0].RotatePacManBody(PacMan.Mouth.Position.North);
                    }
                    break;

                case Keys.Right:
                    if (PacMan.Mouth.Position.East != G_pacMans[0].ActualMouthDirection)
                    {
                        G_pacMans[0].SetPacManDeplacement(PacMan.SpeedOfPacMan, 0);
                        G_pacMans[0].RotatePacManBody(PacMan.Mouth.Position.East);
                    }
                    break;

                case Keys.Down:
                    if (PacMan.Mouth.Position.South != G_pacMans[0].ActualMouthDirection)
                    {
                        G_pacMans[0].SetPacManDeplacement(0, PacMan.SpeedOfPacMan);
                        G_pacMans[0].RotatePacManBody(PacMan.Mouth.Position.South);
                    }
                    break;

                case Keys.Left:
                    if (PacMan.Mouth.Position.West != G_pacMans[0].ActualMouthDirection)
                    {
                        G_pacMans[0].SetPacManDeplacement(-PacMan.SpeedOfPacMan, 0);
                        G_pacMans[0].RotatePacManBody(PacMan.Mouth.Position.West);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion user input
    }
}
