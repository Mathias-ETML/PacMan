using System;
using System.Collections.Generic;
using System.Drawing;
using JsonFileConvertor;
using PacManGame.GameView;
using PacManGame.Entities;
using PacManGame.Map;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PacManGame.Map
{
    public class GameMap : IDisposable
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
            DEBUG
        }
        #endregion enum

        /// <summary>
        /// Attributs
        /// </summary>
        public const int WIDTH = 19;
        public const int HEIGHT = 19;
        private static SolidBrush _solidBrush = new SolidBrush(Color.Empty);
        private FoodMap _foodMap;
        private MapMeaning[,] _gameMap;
        private bool disposedValue = false; // Pour détecter les appels redondants

        #endregion attributs

        #region proprieties
        /// <summary>
        /// Propriety
        /// </summary>

        public MapMeaning[,] GameMapMeaning { get => _gameMap; set => _gameMap = value; }
        public Food[,] FoodsMap { get => _foodMap.FoodsMap; }
        #endregion proprieties

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameMap()
        {
            // new json convertor made by me, its bad BUT it work and i am not a god
            JsonConvertor jsonConvertor = new JsonConvertor(global::PacManGame.Properties.Resources.map, JsonConvertor.Type.Secure, new string[1] {"map"});

            // getting the node because more simpler
            JsonConvertor.JsonNode jsonNode = jsonConvertor.GetElementByName("map");

            // checking the heihgt
            if (jsonNode.GetDataByName<int>("height") != HEIGHT)
            {
                throw new ArgumentException("the height in the json file is not the same as the height in the const field", "MAPHEIGHT");
            }

            // checking the width
            if (jsonNode.GetDataByName<int>("width") != WIDTH)
            {
                throw new ArgumentException("the height in the json file is not the same as the height in the const field", "MAPWIDTH");
            }
            // you never know


            // creating the map with the data
            this._gameMap = new MapMeaning[HEIGHT, HEIGHT];

            // creating the map for the foood
            this._foodMap = new FoodMap(HEIGHT, HEIGHT);

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
        public static void DrawMapRectangle(Graphics graphics, MapMeaning mapMeaning, int x, int y, int width = GameForm.SIZEOFSQUARE, int height = GameForm.SIZEOFSQUARE)
        {
            _solidBrush.Color = _mapDictionary[mapMeaning];

            graphics.FillRectangle(_solidBrush, x, y, width, height);
        }

        public static void DrawRectangle(Graphics graphics, int x, int y, Color color,int width = GameForm.SIZEOFSQUARE, int height = GameForm.SIZEOFSQUARE)
        {
            _solidBrush.Color = color;

            graphics.FillRectangle(_solidBrush, x, y, width, height);
        }
        #endregion Map drawing

        #region Map dictionnary color
        /// <summary>
        /// The color of the square that will be created
        /// </summary>
        private static readonly Dictionary<MapMeaning, Color> _mapDictionary = new Dictionary<MapMeaning, Color>(7)
        {
            { MapMeaning.EMPTY, Color.Black },
            { MapMeaning.WALL, Color.DarkBlue },
            { MapMeaning.ROAD, Color.Black },
            { MapMeaning.FOOD, Color.Black },
            { MapMeaning.BIGFOOD, Color.Black },
            { MapMeaning.TELEPORT, Color.Yellow },
            { MapMeaning.DEBUG, Color.Red }
        };

        public static Dictionary<MapMeaning, Color> MapDictionary => _mapDictionary;
        #endregion Map dictionnary color

        #region Foodmap class
        /// <summary>
        /// Create the map for the food
        /// </summary>
        public class FoodMap : IDisposable
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

            #region IDisposable Support
            private bool disposedValue = false; // Pour détecter les appels redondants

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        
                    }

                    this._foodsMap = null;

                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
        #endregion Foodmap class

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _foodMap.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
