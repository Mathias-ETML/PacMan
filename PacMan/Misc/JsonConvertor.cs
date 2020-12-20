// ==++==
// 
//  Made by Mathias Rogey.
//
//  Published on https://github.com/Mathias-ETML/JsonConvertor
//  with the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007.
//
//  Source code must been available when the software is ditributed.
// 
// ==--==

using System;
using System.Collections.Generic;
using System.Globalization;

/*
 * TODO : Automatic data processing ( hard )
 */
namespace JsonFileConvertor
{
    #region JsonConvertor class
    /// <summary>
    /// JsonConvertor class
    /// </summary>
    public class JsonConvertor : IDisposable
    {
        /* Exemple of how to use the json convertor
         ** 
         *  JsonConvertor jsonConvertor = new JsonConvertor(Properties.Resources.map, new string[1] {"test"});
         *  jsonConvertor.TryCreateElementByName("map");
         *
         *  if (jsonConvertor.TryGetElementByName("map", out JsonConvertor.JsonNode jsonNode))
         *  {
         *      if (jsonNode.TryGetElementByName("height", out JsonConvertor.JsonNode.JsonData jsonData))
         *      {
         *          string name = jsonData.Name;
         *          
         *          // implicit cast
         *          decimal data = jsonData.Data;
         *      }
         *  }
         * 
         *  if (jsonConvertor.TryCreateElementByName("test2", out JsonConvertor.JsonNode jsonNode))
         *  {
         *      if (jsonNode.TryGetElementByName("test22", out JsonConvertor.JsonNode jsonNodeInNode))
         *      {
         *          if (jsonNodeInNode.TryGetElementByName("test221", out JsonConvertor.JsonNode.JsonData dataInJsonNodeInJsonNode))
         *          {
         *              string v = dataInJsonNodeInJsonNode.Data;
         *          }
         *      }
         *  }
         *      
         *  JsonConvertor.JsonNode test = jsonConvertor.GetElementByName("test");    
         *      
         *  float v = test.GetDataByName<float>("test13");
         *  double[] vs = test.GetDataArrayByName<double>("test11");
         *  decimal[,] vss = test.GetDataMultidimentionalArrayByName<decimal>("test12");
         */

        #region attributs
        /// <summary>
        /// Attributs
        /// </summary>
        private string _rawData;
        private List<JsonNode> _jsonNodesList;
        private List<string> _jsonNodesNamesList;
        private Dictionary<string, JsonNode> _jsonNodesNamesDico;
        private bool _disposedValue = false;
        private Type _type;

        public enum Type : byte
        {
            Default,
            Secure,
            Simple
        }
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
        public JsonConvertor(string jsonFileData, Type type = Type.Default)
        {
            if (jsonFileData == null)
            {
                throw new ArgumentNullException("jsonFileData");
            }

            this._type = type;

            this._rawData = jsonFileData;
            this._jsonNodesList = new List<JsonNode>();
            this._jsonNodesNamesList = new List<string>();
            this._jsonNodesNamesDico = new Dictionary<string, JsonNode>();
        }

        /// <summary>
        /// custom constructor
        /// </summary>
        /// <param name="jsonFileData">the data of the json file, raw</param>
        /// <param name="nodes">the nodes you want to create</param>
        /// <param name="showError">show argument excpetion</param>
        public JsonConvertor(string jsonFileData, Type type = Type.Default, string[] nodes = null, bool showError = false)
        {
            if (jsonFileData == null)   
            {
                throw new ArgumentNullException("jsonFileData");
            }

            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            if (type == Type.Simple)
            {
                throw new Exception("You shoud not use this function with a simple json file");
            }

            this._type = type;
            this._rawData = jsonFileData;
            this._jsonNodesList = new List<JsonNode>();
            this._jsonNodesNamesList = new List<string>();
            this._jsonNodesNamesDico = new Dictionary<string, JsonNode>();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] == null)
                {
                    throw new ArgumentException($"A null string was at position {i}");
                }

                if (!TryCreateElementByName(nodes[i]) && showError)
                {
                    throw new ArgumentException($"A json node was not created with the name of {nodes[i]} at position {i}");
                }
            }
        }

        /// <summary>
        /// custom constructor
        /// </summary>
        /// <param name="jsonFileData">the data of the json file, raw</param>
        /// <param name="processFile">if you want to create elements automaticaly</param>
        public JsonConvertor(string jsonFileData, bool processFile, Type type = Type.Default)
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
            if (elementName == null)
            {
                throw new ArgumentNullException("elementName");
            }

            if (_type == Type.Simple)
            {
                throw new Exception("You should use the \"CreateSimpleNode\" Function");
            }

            if (_rawData.Contains($"\"{elementName}\""))
            {
                if (_jsonNodesNamesList.Contains(elementName))
                {
                    throw new ArgumentException($"Node {elementName} was allready created");
                }

                switch (_type)
                {
                    case Type.Default:
                        CreateDefaultJsonConvertor(elementName);
                        break;
                    case Type.Secure:
                        CreateSecureJsonConvertor(elementName);
                        break;
                    case Type.Simple:
                        throw new Exception("You should use the \"CreateSimpleNode\" Function"); // YOU NEVER KNOW

                    default:
                        CreateDefaultJsonConvertor(elementName); // you should not be let the type be null, BAD PRACTICE !
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a secure JsonNode
        /// </summary>
        /// <param name="elementName">the element name</param>
        /// <returns>if node was created</returns>
        private JsonNode CreateSecureJsonConvertor(string elementName)
        {
            // we remove the {} at the start and the end
            if (CleanedSecureData(_rawData.Substring(1, _rawData.Length - 1), elementName, out string cleanedData))
            {
                JsonNode jsonNode = new JsonNode(elementName, cleanedData);
                this._jsonNodesList.Add(jsonNode);
                this._jsonNodesNamesDico.Add(elementName, jsonNode);
                this._jsonNodesNamesList.Add(elementName);
                return jsonNode;
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{elementName}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Create a default JsonNode
        /// </summary>
        /// <param name="elementName">the element name<</param>
        /// <returns>if node was created</returns>
        private JsonNode CreateDefaultJsonConvertor(string elementName)
        {
            // we remove the {} at the start and the end
            if (CleanedDefaultData(_rawData.Substring(1, _rawData.Length - 1), elementName, out string cleanedData))
            {
                JsonNode jsonNode = new JsonNode(elementName, cleanedData);
                this._jsonNodesList.Add(jsonNode);
                this._jsonNodesNamesDico.Add(elementName, jsonNode);
                this._jsonNodesNamesList.Add(elementName);
                return jsonNode;
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{elementName}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// This will create a new "node", how i like to call it, of data, it will take evrything from the id of the json file
        /// </summary>
        /// <param name="elementName">the id of the object</param>
        /// <returns>if the node was created</returns>
        public bool CreateElementByName(string elementName)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException("elementName");
            }

            if (_type == Type.Simple)
            {
                throw new Exception("You should use the \"CreateSimpleNode\" Function");
            }

            if (_rawData.Contains($"\"{elementName}\""))
            {
                if (_jsonNodesNamesList.Contains(elementName))
                {
                    throw new ArgumentException($"Node {elementName} was allready created");
                }

                switch (_type)
                {
                    case Type.Default:
                        CreateDefaultJsonConvertor(elementName);
                        break;
                    case Type.Secure:
                        CreateSecureJsonConvertor(elementName);
                        break;
                    case Type.Simple:
                        break;
                    default:
                        CreateDefaultJsonConvertor(elementName); // you should not be let the type be null, BAD PRACTICE !
                        break;
                }
            }
            else
            {
                throw new ArgumentException($"The JSON file doesn't contain the node \"{elementName}\"");
            }
            return false;
        }

        /// <summary>
        /// This will create a new "node", how i like to call it, of data, it will take evrything from the id of the json file
        /// </summary>
        /// <param name="elementName">the id of the object</param>
        /// <returns>if the node was created</returns>
        public bool TryCreateElementByName(string elementName, out JsonNode jsonNode)
        {
            if (_type == Type.Simple)
            {
                throw new Exception("You should use the \"CreateSimpleNode\" Function");
            }

            if (elementName == null)
            {
                throw new ArgumentNullException("elementName");
            }

            if (_rawData.Contains($"\"{elementName}\""))
            {
                if (_jsonNodesNamesList.Contains(elementName))
                {
                    throw new ArgumentException($"Node {elementName} was allready created");
                }

                switch (_type)
                {
                    case Type.Default:
                        CreateDefaultJsonConvertor(elementName);
                        break;
                    case Type.Secure:
                        CreateSecureJsonConvertor(elementName);
                        break;
                    case Type.Simple:
                        break;
                    default:
                        CreateDefaultJsonConvertor(elementName); // you should not be let the type be null, BAD PRACTICE !
                        break;
                }
            }

            jsonNode = null;
            return false;
        }

        /// <summary>
        /// Private method to create a node whithout checking the data
        /// This is usefull if you allready cleaned the data
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="data">the cleaned data</param>
        private void CreateElement(string name, string data)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (_type == Type.Simple)
            {
                throw new Exception("You should use the \"CreateSimpleNode\" Function");
            }

            JsonNode jsonNode = new JsonNode(name, $"\"{data}\"");
            this._jsonNodesList.Add(jsonNode);
            this._jsonNodesNamesDico.Add(data, jsonNode);
            this._jsonNodesNamesList.Add(data);
        }
        #endregion storing the data

        #region getting the data
        /// <summary>
        /// Get you the data
        /// </summary>
        /// <param name="name">the name of the data</param>
        /// <param name="jsonNode">the node it's in it/param>
        /// <returns>the JsonNode</returns>
        public bool TryGetElementByName(string name, out JsonNode jsonNode)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (_jsonNodesNamesDico.TryGetValue(name, out jsonNode))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Get you the node you want
        /// </summary>
        /// <param name="name">the node you want</param>
        /// <returns>the node</returns>
        public JsonNode GetElementByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (_jsonNodesNamesDico.TryGetValue(name, out JsonNode jsonNode))
            {
                return jsonNode;
            }
            else
            {
                throw new ArgumentException($"The node {jsonNode} was not found");
            }
        }

        /// <summary>
        /// Get you the data
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="T">the type of your variable</typeparam>
        /// <param name="jsonFileRawData">the json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node of your file</param>
        /// <param name="jsonNodes">if your data is in nodes that are in nodes</param>
        /// <returns>the data with the type you want</returns>
        public static T GetDataByJsonFile<T>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataByName<T>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Get you the array of data
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="T">the type of your variable</typeparam>
        /// <param name="jsonFileRawData">the json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node of your file</param>
        /// <param name="jsonNodes">if your data is in nodes that are in nodes</param>
        /// <returns>the data array with the type you want</returns>
        public static T[] GetDataArrayByJsonFile<T>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataArrayByName<T>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Get you the multidimentional array of data
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="T">the type of your variable</typeparam>
        /// <param name="jsonFileRawData">the json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node of your file</param>
        /// <param name="jsonNodes">if your data is in nodes that are in nodes</param>
        /// <returns>the data multidimentional array with the type you want</returns>
        public static T[,] GetDataMultidimentionalArrayByJsonFile<T>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataMultidimentionalArrayByName<T>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Get you the enum you want !
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="Enum">the enum</typeparam>
        /// <param name="jsonFileRawData">the raw json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node</param>
        /// <param name="jsonNodes">the nodes that the data is in</param>
        /// <returns>the enum you want</returns>
        public static Enum GetDataEnumByJsonFile<Enum>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataEnum<Enum>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Get you the array of enum you want !
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="Enum">the enum</typeparam>
        /// <param name="jsonFileRawData">the raw json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node</param>
        /// <param name="jsonNodes">the nodes that the data is in</param>
        /// <returns>the array of enum you want</returns>
        public static Enum[] GetDataEnumArrayByJsonFile<Enum>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataEnumArray<Enum>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }

        /// <summary>
        /// Get you the multidimentional array of enum you want !
        /// The way it work it's that you will input all the nodes where the data is, from the first to the last
        /// This is a good way to get a precise information about something
        /// </summary>
        /// <typeparam name="Enum">the enum</typeparam>
        /// <param name="jsonFileRawData">the raw json file</param>
        /// <param name="jsonData">the name of the data</param>
        /// <param name="jsonNode1">the main node</param>
        /// <param name="jsonNodes">the nodes that the data is in</param>
        /// <returns>the array of enum you want</returns>
        public static Enum[,] GetDataEnumMultidimentionalArrayByJsonFile<Enum>(string jsonFileRawData, string jsonData, string jsonNode1, string[] jsonNodes = null)
        {
            if (jsonFileRawData == null)
            {
                throw new ArgumentNullException("jsonFileRawData");
            }

            if (jsonData == null)
            {
                throw new ArgumentNullException("jsonData");
            }

            if (jsonNode1 == null)
            {
                throw new ArgumentNullException("jsonNode1");
            }

            if (CleanedSecureData(jsonFileRawData, jsonNode1, out string buffer))
            {
                JsonNode jsonNode = new JsonNode(jsonNode1, buffer);

                if (jsonNodes != null)
                {
                    for (int i = 0; i < jsonNodes.Length; i++)
                    {
                        if (!jsonNode.TryGetObjectByName(jsonNodes[i], out jsonNode))
                        {
                            throw new ArgumentException($"Node {jsonNodes[i]} was not found");
                        }
                        // else it's working for you ! yay !
                    }
                }

                return jsonNode.GetDataEnumMultidimentionalArray<Enum>(jsonData);
            }
            else
            {
                throw new ArgumentNullException("Error in JSON convertor :" +
                    $"object \"{jsonNode1}\" was found but not returned proprely, a null was returned");
            }
        }
        #endregion getting the data

        /// <summary>
        /// Clean the data
        /// </summary>
        /// <param name="data">the data</param>
        /// <returns>the data</returns>
        private static string CleanData(string data)
        {
            data = data.Replace(Environment.NewLine, string.Empty);
            data = data.Replace("\t", string.Empty);

            // this is ok
            while (data.Contains("  "))
                data = data.Replace("  ", " ");

            // this is bad, yes, but its secure, maybe replace with regex
            // ps : i know nothing about how to build a regex
            data = data.Replace("[ ", "[");
            data = data.Replace("] ", "]");
            data = data.Replace("{ ", "{");
            data = data.Replace("} ", "}");

            data = data.Replace(" [", "[");
            data = data.Replace(" ]", "]");
            data = data.Replace(" {", "{");
            data = data.Replace(" }", "}");

            data = data.Replace(", ", ",");
            data = data.Replace(": ", ":");

            data = data.Replace(" ,", ",");
            data = data.Replace(" :", ":");

            return data;
        }

        #region data processing
        /// <summary>
        /// Clean the data
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="id">the name of the object</param>
        /// <param name="buffer">the output</param>
        /// <returns>if the retrive was sucessfull</returns>
        private static bool CleanedSecureData(string data, string id, out string buffer)
        {
            #region var
            data = CleanData(data);

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

        /// <summary>
        /// Clean the data
        /// </summary>
        /// <param name="data">raw data</param>
        /// <param name="id">the name of the object</param>
        /// <param name="buffer">the output</param>
        /// <returns>if the retrive was sucessfull</returns>
        private static bool CleanedDefaultData(string data, string id, out string buffer)
        {
            data = CleanData(data);
            buffer = null;

            int start = data.IndexOf(id) - 1;
            
            int startCurlyBracketCount = 0;
            int endCurlyBracketCount = 0;

            #region algo
            for (int i = start; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '{':
                        startCurlyBracketCount++;
                        break;
                    case '}':
                        endCurlyBracketCount++;

                        // here we know that a object will always have the same number of opened and closed bracket
                        if (startCurlyBracketCount == endCurlyBracketCount)
                        {
                            buffer = data.Substring(start, i - start + 1);
                            return true;
                        }
                        break;

                    default:
                        break;
                }
            }
            #endregion algo

            return false;
        }
        #endregion data processing

        #region JsonNode class
        /// <summary>
        /// Json node class
        /// PS : i don't know how it's called
        /// PS : i don't know how it's called
        /// </summary>
        public class JsonNode : IDisposable
        {
            #region Attributs
            /// <summary>
            /// Attributs
            /// </summary>
            private string _rawData;
            private string _name;
            private List<JsonData> _jsonDatasList;
            private Dictionary<string, JsonData> _jsonDataNamesDico;
            private List<string> _jsonDataNamesList;
            private Type _type;

            private Dictionary<string, JsonNode> _jsonNodesNamesDico;
            private List<string> _jsonNodesNamesList;
            private bool _disposedValue = false;
            
            #endregion Attributs

            #region Propriety
            /// <summary>
            /// Propriety
            /// </summary>
            public List<string> JsonDataNamesList { get => _jsonDataNamesList; }
            public List<string> JsonNodeNamesList { get => _jsonNodesNamesList; }
            public string Name { get => _name; }
            #endregion Propriety

            #region Constructor
            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="data">the data of the original json file</param>
            /// <param name="id">the id of the object</param>
            public JsonNode(string name, string data, Type type = Type.Default)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                this._type = type;
                this._rawData = data;
                this._name = name;
                this._jsonDatasList = new List<JsonData>();
                this._jsonDataNamesDico = new Dictionary<string, JsonData>();
                this._jsonDataNamesList = new List<string>();

                this._jsonNodesNamesDico = new Dictionary<string, JsonNode>();
                this._jsonNodesNamesList = new List<string>();

                ProcessData();
            }
            #endregion Constructor

            #region getting the data

            #region region JsonData output
            /// <summary>
            /// here we try to get the data
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found</returns>
            public bool TryGetDataByName(string name, out JsonData jsonData)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out jsonData))
                {
                    
                    return true;
                }

                return false;
            }
            

            /// <summary>
            /// here we try to get the data
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found, else throw a null excpetion</returns>
            public JsonData GetDataByName(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return jsonData;
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");   
                }
            }

            /// <summary>
            /// here we try to get the data with the type you want
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found, else throw a null excpetion</returns>
            public T GetDataByName<T>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.ChangeType<T>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }

            /// <summary>
            /// here we try to get the data of the array
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found, else throw a null excpetion</returns>
            public T[] GetDataArrayByName<T>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.ChangeTypeOfArray<T>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }

            /// <summary>
            /// here we try to get the data of the multidimentional array
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found, else throw a null excpetion</returns>
            public T[,] GetDataMultidimentionalArrayByName<T>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.ChangeTypeOfMultidimentionalArray<T>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }

            /// <summary>
            /// here we try to get the data and transform it to an enum
            /// </summary>
            /// <typeparam name="Enum">the enum</typeparam>
            /// <param name="name">the name of the data</param>
            /// <returns>the enum you want</returns>
            public Enum GetDataEnum<Enum>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.StringToEnum<Enum>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }

            /// <summary>
            /// here we try to get the data array and transform it to an enum
            /// </summary>
            /// <typeparam name="Enum">the enum</typeparam>
            /// <param name="name">the name of the data</param>
            /// <returns>the enum array</returns>
            public Enum[] GetDataEnumArray<Enum>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.StringArrayToEnum<Enum>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }

            /// <summary>
            /// here we try to get the multidimentiona data array and transform it to an enum
            /// </summary>
            /// <typeparam name="Enum">the enum</typeparam>
            /// <param name="name">the name of the data</param>
            /// <returns></returns>
            public Enum[,] GetDataEnumMultidimentionalArray<Enum>(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonDataNamesDico.TryGetValue(name, out JsonData jsonData))
                {
                    return DataTransformation.StringMultidimentionalArrayToEnum<Enum>(jsonData.Data);
                }
                else
                {
                    throw new ArgumentNullException("The data was not found");
                }
            }
            #endregion region JsonData output

            #region JsonNode data output
            /// <summary>
            /// here we try to get the data
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found</returns>
            public bool TryGetObjectByName(string name, out JsonNode jsonNode)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonNodesNamesDico.TryGetValue(name, out jsonNode))
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// here we try to get the data
            /// </summary>
            /// <param name="name">the name of the object</param>
            /// <param name="jsonData">out the object</param>
            /// <returns>if the object was found, else throw a null excpetion</returns>
            public JsonNode GetObjectByName(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (_jsonNodesNamesDico.TryGetValue(name, out JsonNode jsonNode))
                {
                    return jsonNode;
                }
                else
                {
                    throw new ArgumentNullException("The object was not found");
                }
            }
            #endregion JsonNode data output

            #endregion getting the data

            #region processing data
            /// <summary>
            /// We get all the ids and the data of the ids in 
            /// </summary>
            private void ProcessData()
            {
                #region var

                // removing the name with ""
                string buffer = _rawData.Substring(_name.Length + 2, _rawData.Length - _name.Length - 2);

                // removing the :[{}] that make this an object
                if (_type == Type.Secure)
                {
                    buffer = buffer.Substring(3, buffer.Length - 5);
                }
                else
                {
                    buffer = buffer.Substring(2, buffer.Length - 3);
                }


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

                            // here we are checking that we are not marking an object as an array, that will be unfortunate
                            if (buffer[i + 1] != '{')
                            {
                                // checking if we are in an array
                                if (dataStart != 0 && !isArray && buffer[i - 1] != '[')
                                {
                                    isArray = true;
                                    dataStart = i;
                                }
                                else if (buffer[i - 1] == '[' && isArray && !is2DArray)
                                {
                                    is2DArray = true;
                                    dataStart = i - 1;
                                }
                            }
                            else
                            {
                                isObject = true;

                                // we need the "" at the start of the object
                                // well, that's how i made my code
                                nameStart -= 1;

                                // and we reset the brackets
                                // here we put at 1 because we went on them once
                                startBracketCount = 1;

                                startCurlyBracketCount = 0;
                                endBracketCount = 0;
                                endCurlyBracketCount = 0;
                            }

                            break;
                        case ']':
                            endBracketCount++;

                            if (!isObject)
                            {
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
                            }

                            break;

                        case '{':
                            startCurlyBracketCount++;
                            break;

                        case '}':
                            endCurlyBracketCount++;

                            if (startCurlyBracketCount == endCurlyBracketCount && _type == Type.Default)
                            {
                                // the +1 is because we need the ] at the end of the object and the "
                                JsonNode jsonNode = new JsonNode(buffer.Substring(nameStart, i - nameStart + 1), buffer.Substring(nameStart, nameEnd - nameStart + 1));

                                _jsonNodesNamesList.Add(buffer.Substring(nameStart + 1, nameEnd - nameStart - 1));
                                _jsonNodesNamesDico.Add(buffer.Substring(nameStart + 1, nameEnd - nameStart - 1), jsonNode);

                                CreateObjectHolder(buffer.Substring(nameStart + 1, nameEnd - nameStart - 1), jsonNode);

                                // reset
                                nameStart = 0;
                                nameEnd = 0;
                                dataStart = 0;
                                isObject = false;
                            }
                            break;

                        case '\"':

                            if (!isObject)
                            {
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
                            }

                            break;

                        case ':':

                            if (!isObject)
                            {
                                // getting the start of the data
                                if (nameEnd != 0)
                                {
                                    dataStart = i + 1;
                                }
                            }
                            break;

                        case ',':

                            if (!isObject)
                            {
                                // here we are in the case that there is more data, and it's separated with an ','
                                if (dataStart != 0 && !isArray)
                                {
                                    dataEnd = i;

                                    // checking if it's a data type string
                                    if (buffer[dataStart] == '\"')
                                    {
                                        // removing the ""
                                        CreateDataHolder(buffer.Substring(nameStart + 1, nameEnd - nameStart - 2), buffer.Substring(dataStart, dataEnd - dataStart));
                                    }
                                    else
                                    {
                                        CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart), buffer.Substring(dataStart, dataEnd - dataStart));
                                    }

                                    nameStart = 0;
                                    nameEnd = 0;
                                    dataStart = 0;
                                    dataEnd = 0;
                                }
                            }
                            break;
                        
                        default:
                            break;
                    }
                }

                // here we know that we started to create an object but we did not finished it
                if (nameStart != 0 && nameEnd != 0 && dataStart != 0)
                {
                    if (buffer[dataStart] == '\"')
                    {
                        // removing the ""
                        CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart), buffer.Substring(dataStart + 1, buffer.Length - dataStart - 2));
                    }
                    else
                    {
                        CreateDataHolder(buffer.Substring(nameStart, nameEnd - nameStart), buffer.Substring(dataStart, buffer.Length - dataStart));
                    }
                }
                #endregion algo
            }

            #region data creation
            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string data)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                this._jsonDatasList.Add(jsonData);
                this._jsonDataNamesDico.Add(name, jsonData);
                this._jsonDataNamesList.Add(name);
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string[] data)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                this._jsonDatasList.Add(jsonData);
                this._jsonDataNamesDico.Add(name, jsonData);
                this._jsonDataNamesList.Add(name);
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateDataHolder(string name, string[,] data)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                // creating the data holder
                JsonData jsonData = new JsonData(name, data);
                this._jsonDatasList.Add(jsonData);
                this._jsonDataNamesDico.Add(name, jsonData);
                this._jsonDataNamesList.Add(name);
            }

            /// <summary>
            /// Create the data holder
            /// </summary>
            /// <param name="name">the object name</param>
            /// <param name="data">the object data</param>
            private void CreateObjectHolder(string name, JsonNode json)
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }

                // creating the data holder
                JsonData jsonData = new JsonData(name, json);
                this._jsonDatasList.Add(jsonData);
                this._jsonDataNamesDico.Add(name, jsonData);
                this._jsonDataNamesList.Add(name);
            }
            #endregion data creation

            #region data manipulation
            /// <summary>
            /// Convert a json array to string array
            /// </summary>
            /// <param name="rawData">the full json array</param>
            /// <returns>a string array</returns>
            private string[] ConvertDataToArray(string rawData)
            {
                if (rawData == null)
                {
                    throw new ArgumentNullException("rawData");
                }

                // removing the array starter
                rawData = rawData.Replace("[", "");
                rawData = rawData.Replace("]", "");

                // removing the ""
                rawData = rawData.Replace("\"", "");
                
                return rawData.Split(',');
            }

            /// <summary>
            /// Convert a json multidimentional array to multidimentional string array
            /// </summary>
            /// <param name="rawData">the full json array</param>
            /// <returns>a multidimentional string array</returns>
            private string[,] ConvertDataToMultidimentionalArray(string rawData)
            {
                if (rawData == null)
                {
                    throw new ArgumentNullException("rawData");
                }

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
            #endregion data manipulation

            #endregion processing data

            #region Json data class
            /// <summary>
            /// Json data class
            /// </summary>
            public class JsonData : IDisposable, IConvertible, IEquatable<JsonData>
            {
                #region Attributs   
                /// <summary>
                /// Atributs
                /// </summary>
                private string _name;
                private Information _data;
                private static readonly NumberFormatInfo _numberInfoParseNumberWithPoint = CultureInfo.InvariantCulture.NumberFormat;
                private bool _disposedValue = false;
                #endregion Attributs

                #region Proprieties
                /// <summary>   
                /// Proprieties
                /// </summary>
                public string Name { get => _name; }
                public Information Data { get => _data; }
                public JsonNode JsonNode { get => _data.JsonNode; }
                #endregion Proprieties

                #region constructors
                /// <summary>
                /// Custom constructor
                /// </summary>
                /// <param name="name">the name of the id</param>
                /// <param name="data">the data of the id</param>
                public JsonData(string name, string data)
                {
                    if (name == null)
                    {
                        throw new ArgumentNullException("name");
                    }

                    if (data == null)
                    {
                        throw new ArgumentNullException("data");
                    }

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
                    if (name == null)
                    {
                        throw new ArgumentNullException("name");
                    }

                    if (data == null)
                    {
                        throw new ArgumentNullException("data");
                    }

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
                    if (name == null)
                    {
                        throw new ArgumentNullException("name");
                    }

                    if (data == null)
                    {
                        throw new ArgumentNullException("data");
                    }

                    this._name = name;
                    this._data = new Information(data);
                }

                /// <summary>
                /// Custom constructor
                /// </summary>
                /// <param name="name">the name of the id</param>
                /// <param name="data">the data of the id</param>
                public JsonData(string name, JsonNode data)
                {
                    if (name == null)
                    {
                        throw new ArgumentNullException("name");
                    }

                    if (data == null)
                    {
                        throw new ArgumentNullException("data");
                    }

                    this._name = name;
                    this._data = new Information(data);
                }
                #endregion constructors

                #region Information class, hold the data
                /// <summary>
                /// Information class, hold the data of the object
                /// </summary>
                public class Information : IConvertible, IDisposable
                {
                    #region Attributs
                    /// <summary>
                    /// Attributs
                    /// </summary>
                    private string _data;
                    private string[] _array;
                    private string[,] _multiArray;
                    private readonly JsonNode _jsonNode;
                    private int _rank;
                    private bool _disposedValue = false;
                    #endregion Attributs

                    #region Proprieties
                    /// <summary>
                    /// Proprieties
                    /// </summary>
                    public bool IsArray { get => _rank != -1; }
                    public int Rank { get => _rank; }
                    public JsonNode JsonNode { get => _jsonNode; }
                    #endregion Proprieties

                    #region constructors
                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the raw data</param>
                    public Information(string data)
                    {
                        this._data = data;
                        this._rank = -1;
                    }

                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the data but in an array</param>
                    public Information(string[] array)
                    {
                        this._array = array;
                        this._rank = 0;
                    }

                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the data but in an array</param>
                    public Information(string[,] array)
                    {
                        this._multiArray = array;
                        this._rank = 1;
                    }

                    /// <summary>
                    /// custom constructor
                    /// </summary>
                    /// <param name="data">the raw data</param>
                    public Information(JsonNode jsonNode)
                    {
                        this._jsonNode = jsonNode;
                        this._rank = -1;
                    }
                    #endregion constructors

                    #region implicit convertor
                    public static implicit operator string(Information data) => data._data;
                    public static implicit operator string[](Information data) => data._array;
                    public static implicit operator string[,](Information data) => data._multiArray;
                    #endregion implicit convertor

                    #region IConvertible Support
                    public TypeCode GetTypeCode()
                    {
                        return ((IConvertible)_data).GetTypeCode();
                    }

                    public bool ToBoolean(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToBoolean(provider);
                    }

                    public char ToChar(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToChar(provider);
                    }

                    public sbyte ToSByte(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToSByte(provider);
                    }

                    public byte ToByte(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToByte(provider);
                    }

                    public short ToInt16(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToInt16(provider);
                    }

                    public ushort ToUInt16(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToUInt16(provider);
                    }

                    public int ToInt32(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToInt32(provider);
                    }

                    public uint ToUInt32(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToUInt32(provider);
                    }

                    public long ToInt64(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToInt64(provider);
                    }

                    public ulong ToUInt64(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToUInt64(provider);
                    }

                    public float ToSingle(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToSingle(provider);
                    }

                    public double ToDouble(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToDouble(provider);
                    }

                    public decimal ToDecimal(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToDecimal(provider);
                    }

                    public DateTime ToDateTime(IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToDateTime(provider);
                    }

                    public string ToString(IFormatProvider provider)
                    {
                        return _data.ToString();
                    }

                    public object ToType(System.Type conversionType, IFormatProvider provider)
                    {
                        return ((IConvertible)_data).ToType(conversionType, provider);
                    }

                    public override string ToString()
                    {
                        return base.ToString();
                    }

                    public override int GetHashCode()
                    {
                        return base.GetHashCode();
                    }

                    public override bool Equals(object obj)
                    {
                        return base.Equals(obj);
                    }
                    #endregion IConvertible Support

                    #region IDisposable Support
                    /// <summary>
                    /// Dispose support
                    /// Free memory
                    /// </summary>
                    /// <param name="disposing">if you want to dispose managed ressources</param>
                    protected virtual void Dispose(bool disposing)
                    {
                        if (!_disposedValue)
                        {
                            if (disposing)
                            {
                                this._data = null;
                                this._array = null;
                                this._multiArray = null;
                                if (this._jsonNode != null)
                                {
                                    this._jsonNode.Dispose();
                                }
                                this._rank = int.MinValue;
                            }

                            _disposedValue = true;
                        }
                    }

                    /// <summary>
                    /// Dispose support
                    /// </summary>
                    public void Dispose()
                    {
                        Dispose(true);
                        GC.SuppressFinalize(this);
                    }
                    #endregion IDisposable Support
                }
                #endregion Information class, hold the data

                #region IDisposable Support

                /// <summary>
                /// Dispose support
                /// Free memory
                /// </summary>
                /// <param name="disposing">dispose non managed objects</param>
                protected virtual void Dispose(bool disposing)
                {
                    if (!_disposedValue)
                    {
                        if (disposing)
                        {
                            this._name = null;
                            this._data.Dispose();
                        }

                        _disposedValue = true;
                    }
                }

                /// <summary>
                /// Dispose support
                /// </summary>
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                #endregion IDisposable Support

                #region IConvertible support
                public TypeCode GetTypeCode()
                {

                    return ((IConvertible)_data).GetTypeCode();
                }

                public bool ToBoolean(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToBoolean(provider);
                }

                public char ToChar(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToChar(provider);
                }

                public sbyte ToSByte(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToSByte(provider);
                }

                public byte ToByte(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToByte(provider);
                }

                public short ToInt16(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToInt16(provider);
                }

                public ushort ToUInt16(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToUInt16(provider);
                }

                public int ToInt32(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToInt32(provider);
                }

                public uint ToUInt32(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToUInt32(provider);
                }

                public long ToInt64(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToInt64(provider);
                }

                public ulong ToUInt64(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToUInt64(provider);
                }

                public float ToSingle(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToSingle(provider);
                }

                public double ToDouble(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToDouble(provider);
                }

                public decimal ToDecimal(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToDecimal(provider);
                }

                public DateTime ToDateTime(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToDateTime(provider);
                }

                public string ToString(IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToString(provider);
                }

                public object ToType(System.Type conversionType, IFormatProvider provider)
                {
                    return ((IConvertible)_data).ToType(conversionType, provider);
                }
                #endregion IConvertible support

                #region IEquatable<JsonData>
                public bool Equals(JsonData other)
                {
                    return this._data == other._data && this._name == other._name;
                }
                #endregion IEquatable<JsonData>
            }
            #endregion Json data class

            #region IDisposable Support

            /// <summary>
            /// Dispose support
            /// Free memory !
            /// </summary>
            /// <param name="disposing">the non-mamanged objects</param>
            protected virtual void Dispose(bool disposing)
            {
                if (!_disposedValue)
                {
                    if (disposing)
                    {
                        this._rawData = null;
                        this._name = null;

                        foreach (JsonData item in _jsonDatasList)
                        {
                            item.Dispose();
                        }
                        this._jsonDataNamesDico = null;
                        this._jsonDataNamesList = null;

                        this._jsonNodesNamesDico = null;
                        this._jsonNodesNamesList = null;
                    }

                    _disposedValue = true;
                }
            }

            /// <summary>
            /// Dispose support
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion IDisposable Support
        }
        #endregion JsonNode class

        #region IDisposable Support

        /// <summary>
        /// Dispose implement
        /// Free memory
        /// </summary>
        /// <param name="disposing">the unmanaged ressources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (JsonNode item in _jsonNodesList)
                    {
                        item.Dispose();
                    }
                }

                this._rawData = null;
                this._jsonNodesList = null;
                this._jsonNodesNamesDico = null;
                this._jsonNodesNamesList = null;

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose implement
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable Support
    }
    #endregion JsonConvertor class
}
