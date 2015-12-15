using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CleanVSFolders
{
    internal class Program
    {
        private static readonly List<string> ExtensionNameList = new List<string>(50)
                {
                    ".aps", ".bsc", ".cache", ".dep", ".idb", ".ilk", ".ipch", ".log", ".lastbuildstate",
                    ".manifest", ".ncb", ".ncp", ".obj", ".opt", ".pch", ".pdb", ".sdf",
                    ".res", ".sbr", ".scc", ".sdf", ".suo", ".tlog", ".tmp", ".user",
                    ".dll.embed", ".dll.inter", ".exe.embed", ".exe.inter", ".sln.old"
                };

        private static readonly List<string> FileNameList = new List<string>(10)
                {
                    "BuildLog.htm", "UpgradeLog.xml", "TemporaryGeneratedFile"
                };

        private static readonly List<string> DeleteFilePathList = new List<string>();

        private static void Main()
        {
            string appStartupPath = Application.StartupPath;

            if (!SearchFiles(appStartupPath) || !DeleteFiles(DeleteFilePathList))
            {
                Console.WriteLine("Failed");
                Console.ReadKey();
                return;
            }

            DeleteFilePathList.Clear();
            Console.WriteLine("Folder Cleaned");
            Console.ReadKey();
        }

        private static bool SearchFiles(string folderDirectory)
        {
            try
            {
                DeleteFilePathList.AddRange(Directory.GetFiles(folderDirectory));

                // 找出所有目錄
                foreach (string directory in Directory.GetDirectories(folderDirectory))
                {
                    // 針對目前目錄的檔案做處理
                    foreach (string filename in Directory.GetFiles(directory).Where(filename => !DeleteFilePathList.Contains(filename)))
                    {
                        DeleteFilePathList.Add(filename);
                    }

                    // 此目錄處理完再針對每個子目錄做處理
                    SearchFiles(directory);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }

            return true;
        }

        private static bool DeleteFiles(List<string> list)
        {
            if (null == list) return false;

            try
            {
                foreach (var filePath in list)
                {
                    string s = Path.GetExtension(filePath);

                    if (s != null)
                    {
                        s = s.ToLower();

                        if (ExtensionNameList.Contains(s))
                        {
                            File.Delete(filePath);
                        }
                    }

                    if (FileNameList.Contains(Path.GetFileName(filePath)))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }
            return true;
        }
    }
}