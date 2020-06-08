// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// AssemblyHelper provides static methods for working 
    /// with assemblies.
    /// </summary>
    public class AssemblyHelper
    {

        /// <summary>
        /// Gets the path from which the assembly defining a type was loaded.
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <returns>The path.</returns>
        public static string GetAssemblyPath(Type type)
        {
            return GetAssemblyPath(type.Assembly);
        }

        /// <summary>
        /// Gets the path from which an assembly was loaded.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The path.</returns>
        public static string GetAssemblyPath(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;

            if (IsFileUri(codeBase))
                return GetAssemblyPathFromCodeBase(codeBase);

            return assembly.Location;
        }

        /// <summary>
        /// Gets the path to the directory from which an assembly was loaded.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The path.</returns>
        public static string GetDirectoryName( Assembly assembly )
        {
            return System.IO.Path.GetDirectoryName(GetAssemblyPath(assembly));
        }

        /// <summary>
        /// Gets the AssemblyName of an assembly.
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>An AssemblyName</returns>
        public static AssemblyName GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName();
        }

        private static bool IsFileUri(string uri)
        {
            return uri.ToLower().StartsWith(Uri.UriSchemeFile);
        }

        // Public for testing purposes
        public static string GetAssemblyPathFromCodeBase(string codeBase)
        {
            // Skip over the file:// part
            int start = Uri.UriSchemeFile.Length + Uri.SchemeDelimiter.Length;

            bool isWindows = System.IO.Path.DirectorySeparatorChar == '\\';

            if (codeBase[start] == '/') // third slash means a local path
            {
                // Handle Windows Drive specifications
                if (isWindows && codeBase[start + 2] == ':')
                    ++start;  
                // else leave the last slash so path is absolute  
            }
            else // It's either a Windows Drive spec or a share
            {
                if (!isWindows || codeBase[start + 1] != ':')
                    start -= 2; // Back up to include two slashes
            }

            return codeBase.Substring(start);
        }
    }
}
