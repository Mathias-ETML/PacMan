using System;
using System.Collections.Generic;

namespace PacMan
{
    /// <summary>
    /// Data transformation class
    /// </summary>
    public static class DataTransformation
    {
        #region list to 2d array
        /// <summary>
        /// Transform a list of arrays into a multi dimensional array
        /// </summary>
        /// <typeparam name="T">type of returned array</typeparam>
        /// <param name="list">the list</param>
        /// <returns>return multi dimensional array</returns>
        public static T[,] ListToMultidimentionalArray<T>(IList<T[]> list )
        {
            if (list == null)
            {
                throw new ArgumentNullException("List is null");
            }

            if (list.Count == 0)
            {
                throw new ArgumentException("List is empty");
            }

            int listX = list[0].Length;

            // creating the list to return
            T[,] buffer = new T[list.Count, listX];

            // putting the arrays in 1 array
            for (int y = 0; y < list.Count; y++)
            {
                // if not the same length well we can't do anything
                if (list[y].Length != listX)
                {
                    throw new ArgumentException("Arrays must be the same size");
                }

                // putting the data of the array in the list
                for (int x = 0; x < listX; x++)
                {
                    buffer[y, x] = list[y][x];
                }
            }

            return buffer;
        }
        #endregion list to 2d array

        #region 2d string array to 2d enum array
        /// <summary>
        /// make an multi dimentional array of enum
        /// </summary>
        /// <typeparam name="Enum">the enum you want</typeparam>
        /// <param name="array">the string array</param>
        /// <returns>return an multi dimentional array of enum</returns>
        public static Enum[,] MultidimentionalStringArrayToEnum<Enum>(string[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }

            if (array.Length == 0)
            {
                throw new ArgumentNullException("Array is empty");
            }

            Enum[,] buffer = new Enum[array.GetLength(0), array.GetLength(1)];

            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    buffer[y, x] = StringToEnum<Enum>(array[y, x]);
                }
            }

            return buffer;
        }

        /// <summary>
        /// Transfomr a string into an enum
        /// </summary>
        /// <typeparam name="Enum">the enum you want to transform</typeparam>
        /// <param name="data">the string</param>
        /// <returns>return the transformed string</returns>
        public static Enum StringToEnum<Enum>(string data)
        {
            if (data.Contains("."))
            {
                data = data.Split('.')[1];
            }

            return (Enum)System.Enum.Parse(typeof(Enum), data);
        }
        #endregion 2d string array to 2d enum array
    }
}
