using System;
using System.Collections.Generic;
using System.Globalization;

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
                throw new Exception("Array is empty");
            }

            if (array.Rank == 1)
            {
                throw new Exception("Array is not multidimentional");
            }

            // buffer to hold the data
            Enum[,] buffer = new Enum[array.GetLength(0), array.GetLength(1)];

            // transforming the strings into enums
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    buffer[y, x] = StringToEnum<Enum>(array[y, x]);
                }
            }

            return buffer;
        }
        #endregion 2d string array to 2d enum array

        #region enum casting
        /// <summary>
        /// Transform a string into an enum
        /// </summary>
        /// <typeparam name="Enum">the enum you want to transform</typeparam>
        /// <param name="data">the string</param>
        /// <returns>return the transformed string</returns>
        public static Enum StringToEnum<Enum>(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data is null");
            }

            if (data.Length == 0)
            {
                throw new ArgumentNullException("Data is empty");
            }

            // here we only want the left part of the enum
            // example : Type.Error, we only want the " error "
            if (data.Contains("."))
            {
                data = data.Split('.')[1];
            }

            return (Enum)System.Enum.Parse(typeof(Enum), data);
        }
        #endregion enum casting

        #region array type changing
        /// <summary>
        /// Change the type of your array
        /// </summary>
        /// <typeparam name="T">the type you want</typeparam>
        /// <param name="array">the array</param>
        /// <returns>the array with a brand new type</returns>
        public static T[] ChangeTypeOfArray<T>(object[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }

            if (array.Length == 0)
            {
                throw new Exception("Array is empty");
            }

            if (array.Rank == 2)
            {
                throw new Exception("Array is multidimentional");
            }

            //buffer to hold  the data
            T[] buffer = new T[array.Length];

            // converting the data
            for (int x = 0; x < array.Length; x++)
            {
                buffer[x] = ChangeType<T>(array[x]);
            }

            return buffer;
        }
        #endregion array type changing

        #region 2d array type changing
        /// <summary>
        /// change the type of the object
        /// </summary>
        /// <typeparam name="T">the type you want</typeparam>
        /// <param name="array">the array</param>
        /// <returns>the changed array</returns>
        public static T[,] ChangeTypeOfMultidimentionalArray<T>(object[,] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }

            if (array.Length == 0)
            {
                throw new Exception("Array is empty");
            }

            if (array.Rank == 1)
            {
                throw new Exception("Array is not multidimentional");
            }

            //buffer to hold  the data
            T[,] buffer = new T[array.GetLength(0), array.GetLength(1)];

            // converting the data
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    buffer[y, x] = ChangeType<T>(array[y, x]);
                }
            }

            return buffer;
        }
        #endregion 2d array type changing

        #region Misc
        /// <summary>
        /// Change de type of an object
        /// </summary>
        /// <typeparam name="T">the type you want</typeparam>
        /// <param name="data">the data</param>
        /// <returns>the changed object</returns>
        public static T ChangeType<T>(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data is null");
            }

            return (T)Convert.ChangeType(data, typeof(T), CultureInfo.InvariantCulture);
        }
        #endregion Misc
    }
}
