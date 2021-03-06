﻿using System;
using System.Drawing;
using System.Windows.Forms;
using PacManGame.Map;
using PacManGame.Entities;
using PacManGame.Interfaces;
using PacManGame.Controllers;

namespace PacManGame.GameView
{
    public partial class GameForm : Form
    {
        private bool _initialised = false;

        #region Atrributs
        /// <summary>
        /// Attributs
        /// </summary>
        private GameContainer _objectContainer;
        #endregion Attributs

        #region Constructor
        /// <summary>
        /// Custom constructor
        /// </summary>
        public GameForm(GameContainer objectContainer)
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

        #region food map creation
        private void DrawMapAndFood(object sender, PaintEventArgs e)
        {
            // i dont know why but it call this two times
            // quick fix
            // or safety feature
            if (_initialised)
            {
                return;
            }

            Graphics graphics = e.Graphics;

            for (int y = 0; y < GameMap.HEIGHT; y++)
            {
                for (int x = 0; x < GameMap.WIDTH; x++)
                {
                    GameMap.DrawMapRectangle(graphics, _objectContainer.Map.GameMapMeaning[y, x], x * GameMap.SIZEOFSQUARE, y * GameMap.SIZEOFSQUARE);

                    if (_objectContainer.Map.GameMapMeaning[y, x] == GameMap.MapMeaning.FOOD || _objectContainer.Map.GameMapMeaning[y, x] == GameMap.MapMeaning.BIGFOOD)
                    {
                        _objectContainer.Map.NumberOfFoods++;
                        _objectContainer.Map.FoodsMap[y, x] = new Food(graphics, (Food.FoodMeaning)_objectContainer.Map.GameMapMeaning[y, x], x * GameMap.SIZEOFSQUARE, y * GameMap.SIZEOFSQUARE);
                    }
                }
            }

            ((Panel)sender).Paint -= DrawMapAndFood;
            _initialised = true;
        }
        #endregion food map creation

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

                        _objectContainer.PacMans[0].SetPacManDeplacement(0, -Entity.SPEED);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.North);
                    }
                    break;

                case Keys.Right:
                    if (EntityDirection.Direction.East != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(Entity.SPEED, 0);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.East);
                    }
                    break;

                case Keys.Down:
                    if (EntityDirection.Direction.South != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(0, Entity.SPEED);
                        _objectContainer.PacMans[0].RotatePacManBody(EntityDirection.Direction.South);
                    }
                    break;

                case Keys.Left:
                    if (EntityDirection.Direction.West != _objectContainer.PacMans[0].CurrentDirection)
                    {
                        _objectContainer.PacMans[0].SetPacManDeplacement(-Entity.SPEED, 0);
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
