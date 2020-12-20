using System;
using System.Drawing;
using System.Windows.Forms;
using PacManGame.Map;
using PacManGame.Entities;
using PacManGame.Interfaces.IControllerNS;
using PacManGame.Interfaces.IEntityNS;

namespace PacManGame.GameView
{
    public partial class GameForm : Form
    {
        public const int SIZEOFSQUARE = 40;

        #region Atrributs
        /// <summary>
        /// Attributs
        /// </summary>
        private ObjectContainer _objectContainer;
        #endregion Attributs

        #region Constructor
        /// <summary>
        /// Custom constructor
        /// </summary>
        public GameForm(ObjectContainer objectContainer)
        {
            this._objectContainer = objectContainer;

            this.Location = new Point(0, 0);

            InitializeComponent();
        }
        #endregion Construcotr

        #region On start
        public void OnStart()
        {
            // init the first painting of the map
            this.panPanGame.Paint += DrawMapAndFood;
        }
        #endregion OnStart

        #region food map application
        private void DrawMapAndFood(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            for (int y = 0; y < GameMap.HEIGHT; y++)
            {
                for (int x = 0; x < GameMap.WIDTH; x++)
                {
                    GameMap.DrawMapRectangle(graphics, _objectContainer.Map.GameMapMeaning[y, x], x * SIZEOFSQUARE, y * SIZEOFSQUARE);

                    if (_objectContainer.Map.GameMapMeaning[y, x] == Map.GameMap.MapMeaning.FOOD || _objectContainer.Map.GameMapMeaning[y, x] == Map.GameMap.MapMeaning.BIGFOOD)
                    {
                        _objectContainer.Map.FoodsMap[y, x] = new Food(graphics, (Food.FoodMeaning)_objectContainer.Map.GameMapMeaning[y, x], x * SIZEOFSQUARE, y * SIZEOFSQUARE);
                    }
                }
            }

            ((Panel)sender).Paint -= DrawMapAndFood;
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
                    if (EntityDirection.Direction.North != _objectContainer.PacMans[0].CurrentDirection)
                    {

                        _objectContainer.PacMans[0].SetPacManDeplacement(0, -Entities.PacMan.SPEED);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.North);
                    }
                    break;

                case Keys.Right:
                    if (EntityDirection.Direction.East != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(Entities.PacMan.SpeedOfPacMan, 0);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.East);
                    }
                    break;

                case Keys.Down:
                    if (EntityDirection.Direction.South != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(0, Entities.PacMan.SpeedOfPacMan);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.South);
                    }
                    break;

                case Keys.Left:
                    if (EntityDirection.Direction.West != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(-Entities.PacMan.SpeedOfPacMan, 0);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.West);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion user input
    }
}
