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

        public void DrawMapRectangle(Graphics graphics, MapMeaning mapMeaning, int x, int y)
        {
            Color color;

            color = MapDictionary[mapMeaning];

            Rectangle rectangle = new Rectangle(x, y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE);

            graphics.DrawRectangle(new Pen(color, 1), rectangle);

            graphics.FillRectangle(new SolidBrush(color), rectangle);

            //////////////////////////////////////////
        }

        /*
        public struct MapMeaning
        {
            public const char EMPTY = 'e';
            public const char WALL = 'w';
            public const char ROAD = 'r';
            public const char FOOD = 'f';
            public const char BIGFOOD = 'b';
            public const char TELEPORT = 't';
        }
        */

        public enum MapMeaning : byte
        {
            EMPTY,
            WALL,
            ROAD,
            FOOD,
            BIGFOOD,
            TELEPORT
        }

        public Dictionary<MapMeaning, Color> MapDictionary = new Dictionary<MapMeaning, Color>(6)
        {
            { MapMeaning.EMPTY, Color.Black },
            { MapMeaning.WALL, Color.DarkBlue },
            { MapMeaning.ROAD, Color.Black },
            { MapMeaning.FOOD, Color.Black },
            { MapMeaning.BIGFOOD, Color.Black },
            { MapMeaning.TELEPORT, Color.Yellow }
        };

        /*
        public char[,] GameMap { get; set; } = new char[MapHeight, MapWidth]
        {
            {'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'}, //1
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w'}, //2
            {'w', 'b', 'w', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'w', 'b', 'w'}, //3
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w'}, //4
            {'w', 'f', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'f', 'w'}, //5
            {'w', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'w'}, //6
            {'w', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'w'}, //7
            {'e', 'e', 'e', 'w', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'w', 'e', 'e', 'e'}, //8
            {'w', 'w', 'w', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'w', 'w', 'w'}, //9
            {'t', 'r', 'r', 'r', 'f', 'r', 'r', 'w', 'e', 'e', 'e', 'w', 'r', 'r', 'f', 'r', 'r', 'r', 't'}, //10
            {'w', 'w', 'w', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'w', 'w', 'w'}, //11
            {'e', 'e', 'e', 'w', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'w', 'e', 'e', 'e'}, //12
            {'w', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'w'}, //13
            {'w', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'w'}, //14
            {'w', 'f', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'f', 'w'}, //15
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w'}, //16
            {'w', 'b', 'w', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'w', 'b', 'w'}, //17
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w'}, //18
            {'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'}, //19
        };
        */

        public MapMeaning[,] GameMap { get; set; } = new MapMeaning[MapHeight, MapWidth]
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
            {MapMeaning.TELEPORT,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.FOOD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.WALL,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.EMPTY,MapMeaning.WALL,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.FOOD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.ROAD,MapMeaning.TELEPORT,},
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

        public class FoodMap
        {
            public Food[,] tab_foods { get; set; } = new Food[MapWidth, MapHeight];
    }

        public void Die()
        {
            ((IDisposable)this).Dispose();
        }
    }
}
