﻿using System;
using System.Collections.Generic;
using System.Drawing;
using static PacMan.Variables;

namespace PacMan
{
    /// <summary>
    /// Food class
    /// </summary>
    public partial class Food : IDisposable
    {
        #region variables
        #region enum
        public enum FoodMeaning : byte
        {
            FOOD = 3,
            BIGFOOD
        }
        #endregion enum

        /// <summary>
        /// Attributs
        /// </summary>
        private FoodMeaning? _type;
        private bool _disposed = false;
        private Point _foodLocation;
        #endregion variables

        #region propriety
        /// <summary>
        /// Propriety
        /// </summary>
        public bool Disposed { get => _disposed; }
        public FoodMeaning? Type { get => _type; } // can be null if the type is not in the enum
        public Point FoodLocation { get => _foodLocation; }
        #endregion propriety

        #region Food
        /// <summary>
        /// Custom constructor
        /// Create a food
        /// </summary>
        /// <param name="graphics"> the graphics of the panel </param>
        /// <param name="type"> type of food </param>
        /// <param name="x"> x location </param>
        /// <param name="y"> y location </param>
        public Food(Graphics graphics, FoodMeaning type, int x, int y)
        {
            // check if the type is good
            if (type >= (FoodMeaning)3 && type <= (FoodMeaning)4)
            {
                _foodLocation = new Point(x, y);

                _type = type;

                DrawFood(graphics, type, x, y);
            }
            else
            {
                _type = null;
            }
        }

        /// <summary>
        /// Draw food
        /// </summary>
        /// <param name="graphics">the graphics of your object, from this case its a panel</param>
        /// <param name="type">the type of food you want</param>
        /// <param name="x">c location</param>
        /// <param name="y">y location</param>
        public void DrawFood(Graphics graphics, FoodMeaning type, int x, int y)
        {
            graphics.DrawEllipse(FoodColor.pen, x + G_BYTESIZEOFSQUARE / 3, y + G_BYTESIZEOFSQUARE / 3, (G_BYTESIZEOFSQUARE / 10) * (byte)type, (G_BYTESIZEOFSQUARE / 10) * (byte)type);
            graphics.FillEllipse(FoodColor.solidBrush, x + G_BYTESIZEOFSQUARE / 3, y + G_BYTESIZEOFSQUARE / 3, (G_BYTESIZEOFSQUARE / 10) * (byte)type, (G_BYTESIZEOFSQUARE / 10) * (byte)type);
        }
        #endregion Food

        #region memory managment
        /// <summary>
        /// disposing
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
            }
        }

        /// <summary>
        /// disposing
        /// </summary>
        /// <param name="disposing">if you want the other objetcts to be disposed   </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            GC.SuppressFinalize(this);
            _disposed = true;
        }
        #endregion memory managment

        #region relation with food classes
        /// <summary>
        /// Dictionary for the value of food when pacman eat it
        /// </summary>
        public static class Points
        {
            public static readonly Dictionary<Food.FoodMeaning, short> PointForFoods = new Dictionary<Food.FoodMeaning, short>(2)
            {
                {Food.FoodMeaning.FOOD, 100 },
                {Food.FoodMeaning.BIGFOOD, 1000 }
            };
        }

        /// <summary>
        /// Color for the food
        /// </summary>
        internal class FoodColor
        {
            public static Pen pen = new Pen(Color.White, 1);
            public static SolidBrush solidBrush = new SolidBrush(Color.White);
        }
        #endregion relation with food classes
    }
}
