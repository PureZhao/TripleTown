using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LitJson;

namespace Util
{
    public static class FileUtil
    {
        /// <summary>
        /// Comfirm a file wether it is in given directory(deep search)
        /// </summary>
        /// <param name="dir">Director path</param>
        /// <param name="filename">File name with extension</param>
        /// <returns></returns>
        public static bool IsFileExist(string dir, string filename)
        {
            string filePath = Path.Combine(dir, filename);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                return true;
            }
            else
            {
                DirectoryInfo directoryInfo = fileInfo.Directory;
                DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();

                if (subDirectories.Length > 0)
                {
                    foreach (DirectoryInfo info in subDirectories)
                    {
                        if (IsFileExist(info.FullName, filename))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return false;
        }

        public static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}