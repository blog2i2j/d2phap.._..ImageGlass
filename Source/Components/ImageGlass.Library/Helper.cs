﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2017 DUONG DIEU PHAP
Project homepage: http://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using ImageGlass.Library.WinAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ImageGlass.Library
{
    public static class Helper
    {
        /// <summary>
        /// Check if the given form's location is visible on screen
        /// </summary>
        /// <param name="location">The location of form to check</param>
        /// <returns></returns>
        public static bool IsOnScreen(Point location)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                if (screen.WorkingArea.Contains(location))
                {
                    return true;
                }
            }

            return false;
        }


        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);

        /// <summary>
        /// Shorten and ellipsis the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ShortenPath(string path, int length)
        {
            StringBuilder sb = new StringBuilder(length);
            PathCompactPathEx(sb, path, length, 0);
            return sb.ToString();
        }


        /// <summary>
        /// Get filenames by distinct directory
        /// </summary>
        /// <param name="pathList">Path list</param>
        /// <returns></returns>
        public static List<string> GetFilesByDistinctDirs(IEnumerable<string> pathList)
        {
            if (pathList.Count() == 0) return new List<string>();

            var hashedList = new HashSet<string>();

            foreach (var path in pathList)
            {
                // Issue #530: support long file paths by using the magic prefix. Otherwise, File.Exists() always fails.
                var pathToTest = path;
                if (pathToTest.Length > 255)
                    pathToTest = @"\\?\" + path;

                if (File.Exists(pathToTest))
                {
                    string dir;
                    if (Path.GetExtension(path).ToLower() == ".lnk")
                    {
                        var shortcutPath = Shortcuts.GetTargetPathFromShortcut(path);

                        // get the DIR path of shortcut target
                        if (File.Exists(shortcutPath))
                        {
                            dir = Path.GetDirectoryName(shortcutPath);
                        }
                        else if (Directory.Exists(shortcutPath))
                        {
                            dir = shortcutPath;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        dir = Path.GetDirectoryName(path);
                    }

                    hashedList.Add(dir);
                }
                else if (Directory.Exists(pathToTest))
                {
                    hashedList.Add(path);
                }
                else
                {
                    continue;
                }
            }

            return hashedList.ToList();
        }
    }
}
