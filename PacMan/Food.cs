using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PacMan.Variables;

namespace PacMan
{
    public partial class Food : IDisposable
    {
        private FoodMeaning? _type;

        public FoodMeaning? Type { get => _type; } // can be null if the type is not in the enum

        private bool _disposed = false;

        public bool Disposed { get => _disposed; }

        private Point _foodLocation;

        public Point FoodLocation { get => _foodLocation; }

        /// <summary>
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

        public void DrawFood(Graphics graphics, FoodMeaning type, int x, int y)
        {
            graphics.DrawEllipse(FoodColor.pen, x + G_BYTESIZEOFSQUARE / 3, y + G_BYTESIZEOFSQUARE / 3, (G_BYTESIZEOFSQUARE / 10) * (byte)type, (G_BYTESIZEOFSQUARE / 10) * (byte)type);
            graphics.FillEllipse(FoodColor.solidBrush, x + G_BYTESIZEOFSQUARE / 3, y + G_BYTESIZEOFSQUARE / 3, (G_BYTESIZEOFSQUARE / 10) * (byte)type, (G_BYTESIZEOFSQUARE / 10) * (byte)type);
        }

        public enum FoodMeaning : byte
        {
            FOOD = 3,
            BIGFOOD
        }

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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }

    public static class FoodRelation
    {
        public static readonly Dictionary<Food.FoodMeaning, short> PointForFoods = new Dictionary<Food.FoodMeaning, short>(2)
        {
            {Food.FoodMeaning.FOOD, 100 },
            {Food.FoodMeaning.BIGFOOD, 1000 }
        };
    }

    public static class FoodColor
    {
        public static Pen pen = new Pen(Color.White, 1);
        public static SolidBrush solidBrush = new SolidBrush(Color.White);
    }
}
