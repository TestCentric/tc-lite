// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

#if NYI
using System.IO;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="DirectoryInfo"/>s.
    /// </summary>
    internal sealed class DirectoriesComparer : IChainComparer
    {
        public bool? Equal(object x, object y, ref Tolerance tolerance)
        {
            if (!(x is DirectoryInfo) || !(y is DirectoryInfo))
                return null;

            DirectoryInfo xDirectoryInfo = (DirectoryInfo)x;
            DirectoryInfo yDirectoryInfo = (DirectoryInfo)y;

            // Do quick compares first
            if (xDirectoryInfo.Attributes != yDirectoryInfo.Attributes ||
                xDirectoryInfo.CreationTime != yDirectoryInfo.CreationTime ||
                xDirectoryInfo.LastAccessTime != yDirectoryInfo.LastAccessTime)
            {
                return false;
            }

            // TODO: Find a cleaner way to do this
            return new SamePathConstraint(xDirectoryInfo.FullName).ApplyTo(yDirectoryInfo.FullName).IsSuccess;
        }
    }
}
#endif
