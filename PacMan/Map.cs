

using System.Collections.Generic;
using System.Drawing;
using JsonFileConvertor;
using static PacMan.Variables;

namespace PacMan
{
    public class Map
    {
        #region attributs
        #region enum
        /// <summary>
        /// Enum
        /// </summary>
        public enum MapMeaning : byte
        {
            EMPTY,
            WALL,
            ROAD,
            FOOD,
            BIGFOOD,
            TELEPORT,
        }
        #endregion enum

        /// <summary>
        /// Attributs
        /// </summary>
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private static Color _color;
        private FoodMap _foodMap;
        private MapMeaning[,] _gameMap;
        #endregion attributs

        #region proprieties
        /// <summary>
        /// Propriety
        /// </summary>
        public int MapWidth { get => _mapWidth; }
        public int MapHeight { get => _mapHeight; }
        public MapMeaning[,] GameMap { get => _gameMap; set => _gameMap = value; }
        public Food[,] FoodsMap { get => _foodMap.FoodsMap; }
        #endregion proprieties

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Map()
        {
            // new json convertor made by me, its bad BUT it work and i am not a god
            JsonConvertor jsonConvertor = new JsonConvertor(Properties.Resources.map, JsonConvertor.Type.Secure, new string[1] {"map"});

            // getting the node because more simpler
            JsonConvertor.JsonNode jsonNode = jsonConvertor.GetElementByName("map");

            // getting the height
            this._mapHeight = jsonNode.GetDataByName<int>("height");

            // getting the width
            this._mapWidth = jsonNode.GetDataByName<int>("width");

            // creating the map with the data
            this._gameMap = new MapMeaning[_mapHeight, _mapWidth];

            // creating the map for the foood
            this._foodMap = new FoodMap(_mapHeight, _mapWidth);

            // getting the map data
            this._gameMap = jsonNode.GetDataEnumMultidimentionalArray<MapMeaning>("data");

            // memory managment :)
            jsonConvertor.Dispose();
        }
        #endregion constructor

        #region Map drawing
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
            using (SolidBrush solidBrush = new SolidBrush(_color))
            {
                graphics.FillRectangle(solidBrush, x, y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE);
            }
        }
        #endregion Map drawing

        #region Map dictionnary color
        /// <summary>
        /// The color of the square that will be created
        /// </summary>
        public static readonly Dictionary<MapMeaning, Color> MapDictionary = new Dictionary<MapMeaning, Color>(7)
        {
            { MapMeaning.EMPTY, Color.Black },
            { MapMeaning.WALL, Color.DarkBlue },
            { MapMeaning.ROAD, Color.Black },
            { MapMeaning.FOOD, Color.Black },
            { MapMeaning.BIGFOOD, Color.Black },
            { MapMeaning.TELEPORT, Color.Yellow },
        };
        #endregion Map dictionnary color

        #region Foodmap class
        /// <summary>
        /// Create the map for the food
        /// </summary>
        public class FoodMap
        {
            #region Attributs
            /// <summary>
            /// Attributs
            /// </summary>
            private Food[,] _foodsMap;
            #endregion Attributs

            #region proprieties
            /// <summary>
            /// Propriety
            /// </summary>
            public Food[,] FoodsMap { get => _foodsMap; set => _foodsMap = value; }
            #endregion proprieties

            #region constructor
            /// <summary>
            /// Default constructor
            /// </summary>
            public FoodMap(int height, int width)
            {
                this._foodsMap = new Food[height, width];
            }
            #endregion constructor

        }
        #endregion Foodmap class
    }
}
