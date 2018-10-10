namespace Slash.SystemExt.Utils
{
    using System;
    using System.IO;

    public static class FileUtils
    {
        public static void CopyDirectory(string sourcePath, string destinationPath, Func<string, bool> predicate = null)
        {
            sourcePath = Path.GetFullPath(sourcePath);
            destinationPath = Path.GetFullPath(destinationPath);

            Directory.CreateDirectory(destinationPath);

            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (predicate == null || predicate(newPath))
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
                }
            }
        }
    }
}