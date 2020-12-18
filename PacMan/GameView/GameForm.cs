using System;
using System.Drawing;
using System.Windows.Forms;
using PacMan.Map;
using PacMan.Entities;
using PacMan.Interfaces.IControllerNS;

namespace PacMan.GameView
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

            InitializeComponent();
        }
        #endregion Construcotr

        #region On start
        public void OnStart()
        {
            // init the first painting of the map
            this.panPanGame.Paint += DrawFoodMap;
        }
        #endregion OnStart

        #region food map application
        private void DrawFoodMap(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            for (int y = 0; y < GameMap.HEIGHT; y++)
            {
                for (int x = 0; x < GameMap.WIDTH; x++)
                {
                    GameMap.DrawMapRectangle(graphics, _objectContainer.Map.GameMapMeaning[y, x], x * SIZEOFSQUARE, y * SIZEOFSQUARE);

                    _objectContainer.Map.FoodsMap[y, x] = new Food(graphics, (Food.FoodMeaning)_objectContainer.Map.GameMapMeaning[y, x], x * SIZEOFSQUARE, y * SIZEOFSQUARE);
                }
            }

            ((Panel)sender).Paint -= DrawFoodMap;
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
                    if (Entities.PacMan.Mouth.Direction.North != _objectContainer.PacMans[0].ActualMouthDirection)
                    {

                        _objectContainer.PacMans[0].SetPacManDeplacement(0, -Entities.PacMan.SPEED);
                        _objectContainer.PacMans[0].RotatePacManBody(Entities.PacMan.Mouth.Direction.North);
                    }
                    break;

                case Keys.Right:
                    if (Entities.PacMan.Mouth.Direction.East != _objectContainer.PacMans[0].ActualMouthDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(Entities.PacMan.SpeedOfPacMan, 0);
                        _objectContainer.PacMans[0].RotatePacManBody(Entities.PacMan.Mouth.Direction.East);
                    }
                    break;

                case Keys.Down:
                    if (Entities.PacMan.Mouth.Direction.South != _objectContainer.PacMans[0].ActualMouthDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(0, Entities.PacMan.SpeedOfPacMan);
                        _objectContainer.PacMans[0].RotatePacManBody(Entities.PacMan.Mouth.Direction.South);
                    }
                    break;

                case Keys.Left:
                    if (Entities.PacMan.Mouth.Direction.West != _objectContainer.PacMans[0].ActualMouthDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(-Entities.PacMan.SpeedOfPacMan, 0);
                        _objectContainer.PacMans[0].RotatePacManBody(Entities.PacMan.Mouth.Direction.West);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion user input
    }
}
