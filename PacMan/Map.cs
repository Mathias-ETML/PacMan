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
        public const int MapWidth = 21;
        public const int MapHeight = 19;

        public void DrawMapRectangle(Panel panel, char type, int x, int y)
        {
            Color color;

            Graphics graphics = panel.CreateGraphics();

            switch (type)
            {
                case MapMeaning.EMPTY:
                    color = MapColor.EMPTY;
                    break;

                case MapMeaning.WALL:
                    color = MapColor.WALL;
                    break;

                case MapMeaning.ROAD:
                    color = MapColor.ROAD;
                    break;

                case MapMeaning.NORMALFOOD:
                case MapMeaning.BIGFOOD:
                    color = MapColor.BIGFOOD;
                    break;

                case MapMeaning.TELEPORTATION:
                    color = MapColor.TELEPORTATION;
                    break;

                default:
                    return;
            }

            Rectangle rectangle = new Rectangle(x, y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE);

            graphics.DrawRectangle(new Pen(color, 1), rectangle);

            graphics.FillRectangle(new SolidBrush(color), rectangle);
        }

        public struct MapMeaning
        {
            public const char EMPTY = 'e';
            public const char WALL = 'w';
            public const char ROAD = 'r';
            public const char NORMALFOOD = 'f';
            public const char BIGFOOD = 'b';
            public const char TELEPORTATION = 't';
        }

        private struct MapColor
        {
            public readonly static Color EMPTY = Color.Black;
            public readonly static Color WALL = Color.DarkBlue;
            public readonly static Color ROAD = Color.Black;
            public readonly static Color FOOD = Color.White;
            public readonly static Color BIGFOOD = FOOD;
            public readonly static Color TELEPORTATION = Color.Yellow;
        }

        // 17 no border, 19 with border, 21 with tp

        /*
        public char[,] GameMap { get; set; } = new char[MapHeight, MapWidth]
        {
            {'e', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'e' }, //1
            {'e', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'e' }, //2
            {'e', 'w', 'b', 'w', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'w', 'b', 'w', 'e' }, //3
            {'e', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'e' }, //4
            {'e', 'w', 'f', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'f', 'w', 'e' }, //5
            {'e', 'w', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'w', 'e' }, //6
            {'e', 'w', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'w', 'e' }, //7
            {'e', 'e', 'e', 'e', 'w', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'w', 'e', 'e', 'e', 'e' }, //8
            {'w', 'w', 'w', 'w', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'w', 'w', 'w', 'w' }, //9
            
            {'t', 'r', 'r', 'r', 'r', 'f', 'r', 'r', 'w', 'e', 'e', 'e', 'w', 'r', 'r', 'f', 'r', 'r', 'r', 'r', 't' }, //10
            
            {'w', 'w', 'w', 'w', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'w', 'w', 'w', 'w' }, //11
            {'e', 'e', 'e', 'e', 'w', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'w', 'e', 'e', 'e', 'e' }, //12
            {'e', 'w', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'w', 'w', 'w', 'e' }, //13
            {'e', 'w', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'w', 'e' }, //14
            {'e', 'w', 'f', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'f', 'w', 'e' }, //15
            {'e', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'e' }, //16
            {'e', 'w', 'b', 'w', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'f', 'w', 'w', 'b', 'w', 'e' }, //17
            {'e', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', 'e' }, //18
            {'e', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'e' }, //19
        };

        */
        public static char[,] GameMap { get; set; } = new char[MapWidth, MapHeight]
        {
            {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'w', 't', 'w', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', },
            {'w', 'w', 'w', 'w', 'w', 'w', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'w', 'w', 'w', 'w', 'w', 'w', },
            {'w', 'f', 'b', 'f', 'f', 'f', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'f', 'f', 'f', 'b', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'f', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'f', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'f', 'f', 'w', 'f', 'r', 'r', 'w', 'e', 'w', 'r', 'r', 'f', 'w', 'f', 'f', 'f', 'w', },
            {'w', 'w', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'e', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'w', 'w', },
            {'w', 'f', 'f', 'f', 'w', 'f', 'r', 'r', 'w', 'e', 'w', 'r', 'r', 'f', 'w', 'f', 'f', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'r', 'w', 'w', 'w', 'r', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'f', 'f', 'w', 'r', 'r', 'r', 'r', 'r', 'w', 'f', 'f', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'r', 'w', 'w', 'w', 'w', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'r', 'w', 'w', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'w', 'f', 'w', 'f', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'f', 'w', 'f', 'w', 'f', 'w', },
            {'w', 'f', 'b', 'f', 'f', 'f', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'f', 'f', 'f', 'b', 'f', 'w', },
            {'w', 'w', 'w', 'w', 'w', 'w', 'w', 'e', 'w', 'r', 'w', 'e', 'w', 'w', 'w', 'w', 'w', 'w', 'w', },
            {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'w', 't', 'w', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', },
        };
        
    }
}
