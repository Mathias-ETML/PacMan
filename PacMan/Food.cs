using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PacMan.Variables;

namespace PacMan
{
    public partial class Food
    {
        public FoodMeaning? Type { get; }
        private Rectangle FoodDrawing { get; }

        public int intFoodDrawingLocationX { get => FoodDrawing.Location.X; }
        public int intFoodDrawingLocationY { get => FoodDrawing.Location.Y; }

        public int intFoodLocationX { get; }
        public int intFoodLocationY { get; }

        public Food(Graphics graphics, FoodMeaning type, int x, int y)
        {
            if (type >= (FoodMeaning)3 && type <= (FoodMeaning)4)
            {
                intFoodLocationX = x;
                intFoodLocationY = y;

                Type = type;

                FoodDrawing = new Rectangle(x + G_BYTESIZEOFSQUARE / 2 - (byte)type - 1, y + G_BYTESIZEOFSQUARE / 2 - (byte)type - 1, 3 * (byte)type, 3 * (byte)type);
                graphics.DrawEllipse(new Pen(Color.White, 1), FoodDrawing);
                graphics.FillEllipse(new SolidBrush(Color.White), FoodDrawing);
            }
            else
            {
                Type = null;
            }
        }

        public enum FoodMeaning : byte
        {
            FOOD = 3,
            BIGFOOD
        }

        public void Die()
        {
            ((IDisposable)this).Dispose();
        }
    }

    public class FoodType
    {
        // relation beetwen food and points to add
        public Dictionary<Food.FoodMeaning, int> FoodRelation = new Dictionary<Food.FoodMeaning, int>(2)
        {
            {Food.FoodMeaning.FOOD, 100 },
            {Food.FoodMeaning.BIGFOOD, 1000 }
        };
    }

}
