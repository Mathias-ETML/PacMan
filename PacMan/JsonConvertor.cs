using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacMan
{
    /// <summary>
    /// JsonConvertor class
    /// </summary>
    public class JsonConvertor
    {
        /* Exemple of how to use the json convertor
         ** 
         * JsonConvertor jsonConvertor = new JsonConvertor(Properties.Resources.map);
         * jsonConvertor.TryCreateElementByName("map");
         *
         * if (jsonConvertor.TryGetElementByName("map", out JsonConvertor.JsonNode jsonNode))
         * {
         *     if (jsonNode.TryGetElementByName("height", out JsonConvertor.JsonNode.JsonData jsonData))
         *     {
         *         string name = jsonData.Name;
         *         
         *         // implicit cast
         *         decimal data = jsonData.Data;
         *     }
         * }
         * 
        */

        #region attributs
        /// <summary>
        /// Attributs
        /// </summary>
        private string _rawData;
        private List<JsonNode> _jsonNodes;
        private List<string> _jsonNodesNamesList;
        private Dictionary<string, JsonNode> _jsonNodesNamesDico;
        #endregion attributs

        #region Propriety
        /// <summary>
        /// Propriety
        /// </summary>
        public List<string> JsonNodesNamesList { get => _jsonNodesNamesList; }
        #endregion Propriety

        #region constructors
        /// <summary>
        /// custom constructor
        /// </summary>
        /// <param name="jsonFile">the data of the json file, raw</param>
        public JsonConvertor(string jsonFileData)
        {
            this._rawData = jsonFileData;
            this._jsonNodes = new List<JsonNode>();
            this._jsonNodesNamesList = new List<string>();
            this._jsonNodesNamesDico = new Dictionary<string, JsonNode>();
        }

        /// <summary>
        /// custom constructor
        /// </summary>
        /// <param name="jsonFileData">the data of the json file, raw</param>
        /// <param name="processFile">if you want to create elements automaticaly</param>
        public JsonConvertor(string jsonFileData, bool processFile)
        {
            throw new NotImplementedException("Sorry i did not implemented this feature yet.");
            /*
            this._rawData = jsonFileData;
            this._jsonNodes = new List<JsonNode>();

            if (processFile)
            {
                TryCreateElementByName(null);
            }
            */
            
        }
        #endregion constructors

        #region storing the data
        /// <summary>
        /// This will create a new "node", how i like to call it, of data, it will take evrything from the id of the json file
        /// </summary>
        /// <param name="elementName">the id of the object</param>
        /// <returns>if the node was created</returns>
        public bool TryCreateElementByName(string elementName)
        {
            if (_rawData.Contains($"\"{elementName}\""))
            {
                JsonNode jsonNode = new JsonNode(_rawData, $"\"{elementName}\"");
                this._jsonNodes.Add(jsonNode);
                this._jsonNodesNamesDico.Add(elementName, jsonNode);
                this._jsonNodesNamesList.Add(elementName);
                return true;
            }

            return false;
        }
        #endregion storing the data

        #region getting the data
        public bool TryGetElementByName(string name, out JsonNode jsonNode)
        {
            if (_jsonNodesNamesDico.TryGetValue(name, out jsonNode))
            {
                return true;
            }
            
            return false;
        }
        #endregion getting the data

        #region JsonNode class
        /// <summary>
        /// Json node class
        /// PS : i don't know how it's called
        /// </summary>
        public class JsonNode
        {
            #region Attributs
            /// <summary>
            /// Attributs
            /// </summary>
            private string _rawData;
            private string _cleanedData;
            private string _processedData;
            private string _id;
            private List<JsonData> _jsonDatas;
            private Dictionary<string, JsonData> _jsonDataNamesDico;
            private List<string> _jsonDataNamesList;
            #endregion Attributs

            #region Propriety
            /// <summary>
            /// Propriety
            /// </summary>
            public List<string> JsonDataNamesDico { get => _jsonDataNamesList; }
            #endregion Propriety

            #region Constructor
            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="data">the data of the original json file</param>
            /// <param name="id">the id of the object</param>
            public JsonNode(string data, string id)
            {
                this._rawData = data;
                this._id = id;
                this._jsonDatas = new List<JsonData>();
                this._jsonDataNamesDico = new Dictionary<string, JsonData>();
                this._jsonDataNamesList = new List<string>();

                ProcessData();
            }
            #endregion Constructor

            #region getting the data
            /// <summary>
            /// here we try to get the data
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found</returns>
            public bool TryGetElementByName(string name, out JsonData jsonData)
            {
                if (_jsonDataNamesDico.TryGetValue(name, out jsonData))
                {
                    return true;
                }

                return false;
            }
            #endregion getting the data

            #region processing data
            /// <summary>
            /// We get all the ids and the data of the ids in 
            /// </summary>
            private void ProcessData()
            {
                #region var
                int objectNameStart = 0;
                int objectNameEnd = 0;
                int objectDataStart = 0;
                int objectDataEnd = 0;
                bool isArray = false;
                bool is2DArray = false;
                #endregion var

                #region cleaning the data
                _cleanedData = _rawData;
                _cleanedData = _cleanedData.Replace(System.Environment.NewLine, string.Empty);
                _cleanedData = _cleanedData.Replace(" ", String.Empty);
                _processedData = _cleanedData.Split(new string[1] { _id }, StringSplitOptions.None)[1];

                // we start at 2 beacause the first char will be a ':' and a '['
                // and we remove the end because it the delimitation of the object
                _processedData = _processedData.Substring(2, _processedData.Length - 4);
                #endregion cleaning the data

                #region algo, but its a mess
                // yes it's bad
                for (int i = 0; i < _processedData.Length; i++)
                {
                    switch (_processedData[i])
                    {
                        case ':':
                            // here we know its the start of the data
                            if (objectNameEnd != 0)
                            {
                                objectDataStart = i + 1;
                            }
                            break;

                        case '"':
                            // check if we don't start to watch a new object, so we create a new node with data
                            if (objectNameEnd != 0 && !isArray)
                            {
                                objectDataEnd = i - 1;

                                // creating the data a place to hold it
                                JsonData jsonData = new JsonData(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart), _processedData.Substring(objectDataStart, objectDataEnd - objectDataStart));
                                _jsonDatas.Add(jsonData);
                                _jsonDataNamesDico.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart), jsonData);
                                _jsonDataNamesList.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart));

                                // we set the start here because we checked the start of an object
                                objectNameStart = i + 1;

                                // reset
                                objectNameEnd = 0;
                                objectDataStart = 0;
                                objectDataEnd = 0;
                                isArray = false;
                                is2DArray = false;
                                break;
                            }

                            // check if we are checking an object name
                            if (!isArray)
                            {
                                if (objectNameStart != 0)
                                {
                                    objectNameEnd = i;
                                }
                                else
                                {
                                    objectNameStart = i + 1;
                                }
                            }

                            break;

                        case '[':
                            // check if we are in the array of the object
                            if (!isArray && _processedData[i - 1] == ':')
                            {
                                isArray = true;
                                //objectNameStart = i;

                                if (_processedData[i + 1] == '[')
                                {
                                    is2DArray = true;
                                }
                            }
                            break;

                        case ']':
                            // check for the 1d array if it ended
                            if (_processedData[i + 1] != ',' && isArray && !is2DArray)
                            {
                                // the + 1 is because we are right now at the end of the array, so we need to take the next char
                                objectDataEnd = i + 1;
                                isArray = false;

                                string name = _processedData.Substring(objectNameStart, objectNameEnd - objectNameStart);
                                string[] data = ConvertDataToArray(_processedData.Substring(objectDataStart, objectDataEnd - objectDataStart));

                                JsonData jsonData = new JsonData(name, data);
                                _jsonDatas.Add(jsonData);
                                _jsonDataNamesDico.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart), jsonData);
                                _jsonDataNamesList.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart));
                            }

                            // check for the 2d array if it ended
                            if (_processedData[i + 1] == ']' && is2DArray)
                            {
                                // the + 2 is because we are right now at the end of the array, so we need to take the next char and the next one
                                objectDataEnd = i + 2;
                                isArray = false;
                                is2DArray = false;

                                string name = _processedData.Substring(objectNameStart, objectNameEnd - objectNameStart);
                                string[,] data = ConvertDataToMultidimentionalArray(_processedData.Substring(objectDataStart, objectDataEnd - objectDataStart));

                                JsonData jsonData = new JsonData(name, data);
                                _jsonDatas.Add(jsonData);
                                _jsonDataNamesDico.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart), jsonData);
                                _jsonDataNamesList.Add(_processedData.Substring(objectNameStart, objectNameEnd - objectNameStart));
                            }
                            break;
                        
                        default:
                            break;
                    }
                }
                #endregion algo, but its a mess
            }

            /// <summary>
            /// Convert a json array to string array
            /// </summary>
            /// <param name="rawData">the full json array</param>
            /// <returns>a string array</returns>
            private string[] ConvertDataToArray(string rawData)
            {
                string[] buffer;
                
                // removing the array starter
                rawData = rawData.Replace("[", "");
                rawData = rawData.Replace("]", "");

                // removing the ""
                rawData = rawData.Replace("\"", "");

                // putting data into the array
                buffer = rawData.Split(',');

                return buffer;
            }

            /// <summary>
            /// Convert a json multidimentional array to multidimentional string array
            /// </summary>
            /// <param name="rawData">the full json array</param>
            /// <returns>a multidimentional string array</returns>
            private string[,] ConvertDataToMultidimentionalArray(string rawData)
            {
                List<string[]> buffer = new List<string[]>();
                string[] dataBuffer;

                // removing the start of the arrays
                rawData = rawData.Replace("[", "");

                // replacing the arrays separator
                rawData = rawData.Replace("],", "]");

                // removing the "" of the data
                rawData = rawData.Replace("\"", "");

                // removing the end of the multidimentional array
                rawData = rawData.Substring(0, rawData.Length - 2);

                // splitting each array in array
                dataBuffer = rawData.Split(']');

                // putting each array in a list and splitting them
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    buffer.Add(dataBuffer[i].Split(','));
                }

                return DataTransformation.ListToMultidimentionalArray<string>(buffer);
            }

            #endregion processing data

            #region Json data class
            /// <summary>
            /// Json data class
            /// </summary>
            public class JsonData
            {
                #region Attributs
                /// <summary>
                /// Atributs
                /// </summary>
                private string _name;
                private Information _data;
                #endregion Attributs

                #region Proprieties
                /// <summary>   
                /// Proprieties
                /// </summary>
                public string Name { get => _name; }
                public Information Data { get => _data; }
                #endregion Proprieties

                #region constructors
                /// <summary>
                /// Custom constructor
                /// </summary>
                /// <param name="name">the name of the id</param>
                /// <param name="data">the data of the id</param>
                public JsonData(string name, string data)
                {
                    this._name = name;
                    this._data = new Information(data);
                }

                /// <summary>
                /// Custom constructor
                /// </summary>
                /// <param name="name">the name of the id</param>
                /// <param name="data">the data of the id</param>
                public JsonData(string name, string[] data)
                {
                    this._name = name;
                    this._data = new Information(data);
                }

                /// <summary>
                /// Custom constructor
                /// </summary>
                /// <param name="name">the name of the id</param>
                /// <param name="data">the data of the id</param>
                public JsonData(string name, string[,] data)
                {
                    this._name = name;
                    this._data = new Information(data);
                }
                #endregion constructors

                #region Information class, hold the data
                /// <summary>
                /// Information class, hold the data of the object
                /// </summary>
                public class Information
                {
                    #region Attributs
                    /// <summary>
                    /// Attributs
                    /// </summary>
                    private string _data;
                    private string[] _array;
                    private string[,] _multiArray;
                    private System.Type _type;
                    #endregion Attributs

                    #region Proprieties
                    /// <summary>
                    /// Proprieties
                    /// </summary>
                    public System.Type Type { get => _type; }
                    public bool IsArray { get => _array != null; }
                    #endregion Proprieties

                    #region Type casting
                    /// <summary>
                    /// Implicit cast for int type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator int(Information data) => int.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for long type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator long(Information data) => long.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for ulong type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator ulong(Information data) => ulong.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for float type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator float(Information data) => float.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for double type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double(Information data) => double.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for decimal type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal(Information data) => decimal.Parse(data._data);

                    /// <summary>
                    /// Implicit cast for strings
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator string(Information data) => data._data;
                    #endregion Type casting

                    #region Type array casting
                    /// <summary>
                    /// Implicit cast for arrays
                    /// </summary>
                    /// <param name="data">the data</param>
                    //public static implicit operator Array[](Information data) => data._array;

                    /// <summary>
                    /// Implicit cast for int type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator int[](Information data) => Array.ConvertAll<string, int>(data._array, item => int.Parse(item));

                    /// <summary>
                    /// Implicit cast for long type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator long[] (Information data) => Array.ConvertAll<string, long>(data._array, item => long.Parse(item));

                    /// <summary>
                    /// Implicit cast for ulong type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator ulong[] (Information data) => Array.ConvertAll<string, ulong>(data._array, item => ulong.Parse(item));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator float[] (Information data) => Array.ConvertAll<string, float>(data._array, item => float.Parse(item));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double[] (Information data) => Array.ConvertAll<string, double>(data._array, item => double.Parse(item));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal[] (Information data) => Array.ConvertAll<string, decimal>(data._array, item => decimal.Parse(item));

                    #endregion Type array casting

                    #region Type 2darray casting
                    /*
                    /// <summary>
                    /// Implicit cast for arrays
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator Array[](Information data) => data._array;

                    /// <summary>
                    /// Implicit cast for int type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator int[] (Information data) => Array.ConvertAll<string, int>(data._array, item => int.Parse(item));

                    /// <summary>
                    /// Implicit cast for long type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator long[] (Information data) => Array.ConvertAll<string, long>(data._array, item => long.Parse(item));

                    /// <summary>
                    /// Implicit cast for ulong type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator ulong[] (Information data) => Array.ConvertAll<string, ulong>(data._array, item => ulong.Parse(item));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator float[] (Information data) => Array.ConvertAll<string, float>(data._array, item => float.Parse(item));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double[] (Information data) => Array.ConvertAll<string, double>(data._array, item => double.Parse(item));
                    
                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal[] (Information data) => Array.ConvertAll<string, decimal>(data._array, item => decimal.Parse(item));
                    */

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator string[,] (Information data) => data._multiArray;
                    #endregion Type 2darray casting

                    #region constructors
                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the raw data</param>
                    public Information(string data)
                    {
                        this._data = data;
                        SetType();
                    }

                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the data but in an array</param>
                    public Information(string[] array)
                    {
                        this._array = array;
                        this._data = array[0].ToString();
                        SetType();

                        // reset because we got the type
                        this._data = null;
                    }

                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the data but in an array</param>
                    public Information(string[,] array)
                    {
                        this._multiArray = array;
                        this._data = array[0,0].ToString();
                        SetType();

                        // reset because we got the type
                        this._data = null;
                    }
                    #endregion constructors

                    #region Type setting
                    /// <summary>
                    /// We check what type the data is
                    /// </summary>
                    private void SetType()
                    {
                        if (Regex.IsMatch(_data, @"^[a-zA-Z]+$"))
                        {
                            this._type = typeof(string);
                            return;
                        }

                        if (int.TryParse(_data, out _))
                        {
                            this._type = typeof(int);
                            return;
                        }

                        // don't forget you need "," to work, not a point "."
                        // we put here the foat check because its the most common type after the int ( i think )
                        if (float.TryParse(_data, out _))
                        {
                            this._type = typeof(float);
                            return;
                        }

                        if (long.TryParse(_data, out _))
                        {
                            this._type = typeof(long);
                            return;
                        }

                        if (ulong.TryParse(_data, out _))
                        {
                            this._type = typeof(long);
                            return;
                        }

                        // don't forget you need "," to work, not a point "."
                        if (double.TryParse(_data, out _))
                        {
                            this._type = typeof(double);
                            return;
                        }

                        // don't forget you need "," to work, not a point "."
                        if (decimal.TryParse(_data, out _))
                        {
                            this._type = typeof(decimal);
                            return;
                        }

                        // else this is the default type of data
                        this._type = typeof(string);
                    }

                    #endregion Type setting
                }
                #endregion Information class, hold the data
            }
            #endregion Json data class
        }
        #endregion JsonNode class
    }
}
