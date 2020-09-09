using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public class Map
    {
        public const int MapWidth = 19;
        public const int MapHeight = 19;

        private static Color _color;

        /// <summary>
        /// Create a square with the propriety of the map
        /// </summary>
        /// <param name="graphics"> the graphics of the panel </param>
        /// <param name="mapMeaning"> what type of square to you want, you can pick it from the map </param>
        /// <param name="x"> x location </param>
        /// <param name="y"> y location </param>
        public static void DrawMapRectangle(Graphics graphics, MapMeaning mapMeaning, int x, int y)
        {
            _color = MapDictionary[mapMeaning];

            // auto dispose
            /*
            using (Pen pen = new Pen(Color.Pink, 5))
            {
                graphics.DrawRectangle(pen, x, y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE);
            }
            */
            // auto dispose
            using (SolidBrush solidBrush = new SolidBrush(_color))
            {
                graphics.FillRectangle(solidBrush, x, y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE);
            }
        }

        public enum MapMeaning : byte
        {
            EMPTY,
            WALL,
            ROAD,
            FOOD,
            BIGFOOD,
            TELEPORT,
            DEBUG
        }

        /// <summary>
        /// The color of the square that will be created
        /// </summary>
        public static readonly Dictionary<MapMeaning, Color> MapDictionary = new Dictionary<MapMeaning, Color>(7)
        {
            { MapMeaning.EMPTY, Color.Black },
            { MapMeaning.WALL, Color.DarkBlue },
            { MapMeaning.ROAD, Color.Black },
            { MapMeaning.FOOD, Color.Black },
            { MapMeaning.BIGFOOD, Color.Pink },
            { MapMeaning.TELEPORT, Color.Yellow },
            { MapMeaning.DEBUG, Color.Red }
        };

        /// <summary>
        /// The map of the game
        /// </summary>
        public static MapMeaning[,] GameMap = new MapMeaning[MapHeight, MapWidth]
        {
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.BIGFOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.BIGFOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
            {MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,},
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.TELEPORT,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.FOOD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.FOOD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.TELEPORT,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
            {MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,},
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.BIGFOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.BIGFOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.FOOD,MapMeaning.WALL,},
            {MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,MapMeaning.WALL,},
        };

        /// <summary>
        /// Create the map for the food
        /// </summary>
        public static class FoodMap
        {
            public static Food[,] tab_foods { get; set; } = new Food[MapWidth, MapHeight];
        }
    }
}
