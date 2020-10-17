using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
 * TODO : GENERAL ALGO
 * TODO : what happen if there is more after the json data, like a " sous-section "
 * TODO : TRY PARSE WITH .
 * TODO : OBJECT IN OBJECT
 */
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
                // we remove the {} at the start and the end
                if(CleanedData(_rawData.Substring(1, _rawData.Length - 1), elementName, out string cleanedData))
                {
                    JsonNode jsonNode = new JsonNode(cleanedData, $"\"{elementName}\"");
                    this._jsonNodes.Add(jsonNode);
                    this._jsonNodesNamesDico.Add(elementName, jsonNode);
                    this._jsonNodesNamesList.Add(elementName);
                    return true;
                }
                else
                {
                    throw new ArgumentNullException("Error in JSON convertor :" +
                        $"object \"{elementName}\" was found but not returned proprely, a null was returned");
                }

            }

            return false;
        }
        #endregion storing the data

        #region data processing
        /// <summary>
        /// Clean the data do you have a more friends string of char
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="id">the name ob the object</param>
        /// <param name="buffer">the output</param>
        /// <returns>if the retrive was sucessfull</returns>
        private bool CleanedData(string data, string id, out string buffer)
        {
            #region var
            data = data.Replace(Environment.NewLine, string.Empty);
            data = data.Replace(" ", "");
            int start = data.IndexOf(id) - 1;

            int startCurlyBracketCount = 0;
            int endCurlyBracketCount = 0;

            int startBracketCount = 0;
            int endBracketCount = 0;
            #endregion var

            #region algo
            for (int i = start; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '[':
                        startBracketCount++;
                        break;
                    case ']':
                        endBracketCount++;

                        // here we know that a object will always have the same number of opened and closed bracket
                        if (startBracketCount == endBracketCount && startCurlyBracketCount == endCurlyBracketCount)
                        {
                            buffer = data.Substring(start, i - start + 1);
                            return true;
                        }
                        break;
                    case '{':
                        startCurlyBracketCount++;
                        break;
                    case '}':
                        endCurlyBracketCount++;
                        break;

                    default:
                        break;
                }
            }
            #endregion algo

            buffer = null;
            return false;
        }
        #endregion data processing

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
            private string _name;
            private List<JsonData> _jsonDatas;
            private Dictionary<string, JsonData> _jsonDataNamesDico;
            private List<string> _jsonDataNamesList;
            #endregion Attributs

            #region Propriety
            /// <summary>
            /// Propriety
            /// </summary>
            public List<string> JsonDataNamesDico { get => _jsonDataNamesList; }
            public string Name { get => _name; }
            #endregion Propriety

            #region Constructor
            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="data">the data of the original json file</param>
            /// <param name="id">the id of the object</param>
            public JsonNode(string data, string name)
            {
                this._rawData = data;
                this._name = name;
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
                string buffer = _rawData.Substring(_name.Length, _rawData.Length - _name.Length);
                buffer = buffer.Substring(3, buffer.Length - 5);

                int nameStart = 0;
                int nameEnd = 0;
                int dataStart = 0;
                int dataEnd = 0;
                bool isArray = false;
                bool is2DArray = false;
                bool isObject = false;

                int startCurlyBracketCount = 0;
                int endCurlyBracketCount = 0;

                int startBracketCount = 0;
                int endBracketCount = 0;
                #endregion var

                #region algo
                for (int i = 0; i < buffer.Length; i++)
                {
                    switch (buffer[i])
                    {
                        case '[':
                            startBracketCount++;

                            // checking if we are in an array
                            if (dataStart != 0 && !isArray && buffer[i - 1] != '[')
                            {
                                isArray = true;
                                dataStart = i;
                            }
                            else if (buffer[i - 1] == '[' && isArray && !is2DArray)
                            {
                                is2DArray = true;
                                char[] v = buffer.ToCharArray();
                                dataStart = i - 1;
                            }

                            break;
                        case ']':

                            // ending the array section
                            if (isArray && !is2DArray)
                            {
                                dataEnd = i;

                                // creating the data holder
                                CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart),
                                    ConvertDataToArray(buffer.Substring(dataStart, dataEnd - dataStart)));

                                // reset
                                nameStart = 0;
                                nameEnd = 0;
                                dataStart = 0;
                                dataEnd = 0;
                                isArray = false;
                                
                            }
                            else if (is2DArray && buffer[i - 1] == ']')
                            {
                                // we need the ]]
                                dataEnd = i + 1;

                                // creating the data holder
                                CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart),
                                    ConvertDataToMultidimentionalArray(buffer.Substring(dataStart, dataEnd - dataStart)));

                                // reset
                                nameStart = 0;
                                nameEnd = 0;
                                dataStart = 0;
                                dataEnd = 0;
                                isArray = false;
                                is2DArray = false;
                                
                            }
                            endBracketCount++;
                            break;
                        case '{':
                            startCurlyBracketCount++;
                            break;
                        case '}':
                            endCurlyBracketCount++;
                            break;

                        case '\"':

                            // getting the object name
                            if (nameEnd == 0)
                            {
                                if (nameStart == 0)
                                {
                                    nameStart = i + 1;
                                }
                                // geting the object end
                                else if (buffer[i + 1] == ':' && nameStart != 0)
                                {
                                    nameEnd = i;
                                }
                            }
                            // else we are in data with string

                            break;

                        case ':':

                            // getting the start of the data
                            if (nameEnd != 0)
                            {
                                dataStart = i + 1;
                            }
                            break;

                        case ',':

                            // here we are in the case that there is more data, and it's separated with an ','
                            if (dataStart != 0 && !isArray)
                            {
                                dataEnd = i;

                                CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart), buffer.Substring(dataStart, dataEnd - dataStart));
                                nameStart = 0;
                                nameEnd = 0;
                                dataStart = 0;
                                dataEnd = 0;
                            }
                            break;
                        
                        default:
                            break;
                    }
                }

                // here we know that we started to create an object but we did not finished it
                if (nameStart != 0 && nameEnd != 0 && dataStart != 0)
                {
                    CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart), buffer.Substring(dataStart, buffer.Length - dataStart));
                }
                #endregion algo
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string data)
            {
                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                _jsonDatas.Add(jsonData);
                _jsonDataNamesDico.Add(name, jsonData);
                _jsonDataNamesList.Add(name);
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string[] data)
            {
                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                _jsonDatas.Add(jsonData);
                _jsonDataNamesDico.Add(name, jsonData);
                _jsonDataNamesList.Add(name);
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string[,] data)
            {
                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                _jsonDatas.Add(jsonData);
                _jsonDataNamesDico.Add(name, jsonData);
                _jsonDataNamesList.Add(name);
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

                // sous-section
                private List<JsonData> _jsonDatas;
                private Dictionary<string, JsonData> _jsonDataNamesDico;
                private List<string> _jsonDataNamesList;
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
                    private int _rank;
                    private System.Type _type;
                    #endregion Attributs

                    #region Proprieties
                    /// <summary>
                    /// Proprieties
                    /// </summary>
                    public System.Type Type { get => _type; }
                    public bool IsArray { get => _rank != -1; }
                    public int Rank { get => _rank; }
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
                    public static implicit operator float(Information data) => float.Parse(data._data, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol);

                    /// <summary>
                    /// Implicit cast for double type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double(Information data) => double.Parse(data._data, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol);

                    /// <summary>
                    /// Implicit cast for decimal type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal(Information data) => decimal.Parse(data._data, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol);

                    /// <summary>
                    /// Implicit cast for strings
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator string(Information data) => data._data;

                    #endregion Type casting

                    #region Type array casting
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
                    public static implicit operator float[] (Information data) => Array.ConvertAll<string, float>(data._array, item => float.Parse(item, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double[] (Information data) => Array.ConvertAll<string, double>(data._array, item => double.Parse(item, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol));

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal[] (Information data) => Array.ConvertAll<string, decimal>(data._array, item => decimal.Parse(item, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowCurrencySymbol));

                    /// <summary>
                    /// Implicit cast for strings
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator string[](Information data) => data._array;

                    #endregion Type array casting

                    #region Type 2d array casting
                    /// <summary>
                    /// Implicit cast for int type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator int[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<int>(data._multiArray);

                    
                    /// <summary>
                    /// Implicit cast for long type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator long[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<long>(data._multiArray);

                    /// <summary>
                    /// Implicit cast for ulong type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator ulong[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<ulong>(data._multiArray);

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator float[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<float>(data._multiArray);

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator double[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<double>(data._multiArray);

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator decimal[,] (Information data) => DataTransformation.ChangeTypeOfMultidimentionalArray<decimal>(data._multiArray);

                    /// <summary>
                    /// Implicit cast for flont type
                    /// </summary>
                    /// <param name="data">the data</param>
                    public static implicit operator string[,] (Information data) => data._multiArray;
                    #endregion Type 2d array casting

                    #region constructors
                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the raw data</param>
                    public Information(string data)
                    {
                        this._data = data;
                        this._rank = -1;
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
                        this._rank = 0;
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
                        this._rank = 1;
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
                        // we put here the float check because its the most common type after the int ( i think )
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
