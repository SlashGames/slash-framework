// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.SystemExt.Utils
{
    using System;
    using System.Text.RegularExpressions;

#if !WINDOWS_STORE && !WINDOWS_PHONE
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
#else
    using System.Reflection;
#endif

    /// <summary>
    ///   Extension methods for classes of the System namespace.
    /// </summary>
    public static class SystemExtensions
    {
        #region Public Methods and Operators

#if !WINDOWS_STORE && !WINDOWS_PHONE
        /// <summary>
        ///   Perform a deep Copy of the object.
        ///   Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
        ///   Provides a method for performing a deep copy of an object.
        ///   Binary Serialization is used to perform the copy.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
#endif

        /// <summary>
        ///   Returns the full name of the specified type without any assembly version info.
        ///   The reason why this method is needed is that a generic type's FullName contains
        ///   the full AssemblyQualifiedName of its item type.
        /// </summary>
        /// <param name="type">Type to get full name for.</param>
        /// <returns>Full name of specified type without additional info of the assembly.</returns>
        public static string FullNameWithoutAssemblyInfo(this Type type)
        {
#if WINDOWS_STORE
            return !type.GetTypeInfo().IsGenericType ? type.FullName : RemoveAssemblyInfo(type.FullName);
#else
            return !type.IsGenericType ? type.FullName : RemoveAssemblyInfo(type.FullName);
#endif
        }

        /// <summary>
        ///   Checks if the specified value is between the specified lower and higher bound (exclusive).
        /// </summary>
        /// <typeparam name="T"> Type of objects to compare. </typeparam>
        /// <param name="value"> Value to compare. </param>
        /// <param name="low"> Lower bound. </param>
        /// <param name="high"> Higher bound (exclusive). </param>
        /// <returns> True if the value is between the lower and higher bound (exclusive); otherwise, false. </returns>
        public static bool IsBetween<T>(this T value, T low, T high) where T : IComparable<T>
        {
            return value.CompareTo(low) >= 0 && value.CompareTo(high) < 0;
        }

        /// <summary>
        ///   Removes all assembly info from the specified type name.
        /// </summary>
        /// <param name="typeName">Type name to remove assembly info from.</param>
        /// <returns>Type name without assembly info.</returns>
        public static string RemoveAssemblyInfo(string typeName)
        {
            // Get start of "Version=..., Culture=..., PublicKeyToken=..." string.
            int versionIndex = typeName.IndexOf("Version=", StringComparison.Ordinal);

            if (versionIndex >= 0)
            {
                // Get end of "Version=..., Culture=..., PublicKeyToken=..." string for generics.
                int endIndex = typeName.IndexOf(']', versionIndex);

                // Get end of "Version=..., Culture=..., PublicKeyToken=..." string for non-generics.
                endIndex = endIndex >= 0 ? endIndex : typeName.Length;

                // Remove version info.
                typeName = typeName.Remove(versionIndex - 2, endIndex - versionIndex + 2);
            }

            return typeName;
        }

        /// <summary>
        ///   Splits the string, inserting spaces before each capital letter except the first one.
        /// </summary>
        /// <param name="s">String to split.</param>
        /// <returns>Split string.</returns>
        public static string SplitByCapitalLetters(this string s)
        {
            Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        /// <summary>
        ///   Returns the specified string, converting the first letter to upper case.
        /// </summary>
        /// <param name="s">String to convert the first letter of.</param>
        /// <returns>Specified string with first letter in upper case.</returns>
        public static string FirstLetterToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length > 1)
            {
                return char.ToUpper(s[0]) + s.Substring(1);
            }

            return s.ToUpper();
        }

        #endregion
    }
}