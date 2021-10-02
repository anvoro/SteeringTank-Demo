
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tank
{
    public static class CodeAnalyzer
    {
        private const string rootFolder = "Assets";
        private const string p504ScrptsFolder = "Scripts";
        private const string rootNamespace = "Tank";

        private const string yellow = "<color=yellow>";
        private const string green = "<color=green>";
        private const string colorend = "</color>";

        private class FolderStatistic
        {
            public int LinesCount { get; private set; }
            public int FilesCount { get; private set; }
            public void AddFile(int linesCount)
            {
                FilesCount++;
                LinesCount += linesCount;
            }
        }

        [MenuItem("P504/Analyze Code")]
        public static void AnalyzeCode()
        {
            Dictionary<string, FolderStatistic> statisticPerFolder = new Dictionary<string, FolderStatistic>();

            foreach (string filePath in GetAllCSharpFiles(rootFolder))
            {
                string folder = GetFolderFromPath(filePath, 1);
                string[] lines = File.ReadAllLines(filePath);

                if (folder == p504ScrptsFolder)
                {
                    bool analyze = true;

                    if (analyze)
                    {
                        AnalyzeCSharpFile(filePath, ref lines);
                        File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
                    }
                }

                AddStatistic(statisticPerFolder, folder, lines.Length);
                AddStatistic(statisticPerFolder, rootFolder, lines.Length);
            }

            PrintStatistic(statisticPerFolder);
        }

        private static void AddStatistic(Dictionary<string, FolderStatistic> statisticPerFolder, string folder, int linesCount)
        {
            if (statisticPerFolder.ContainsKey(folder) == false)
            {
                statisticPerFolder.Add(folder, new FolderStatistic());
            }

            statisticPerFolder[folder].AddFile(linesCount);
        }

        private static void AnalyzeCSharpFile(string filePath, ref string[] lines)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                bool prevLineEmpty = string.IsNullOrEmpty(lines[i - 1]);
                bool thisLineEmpty = string.IsNullOrEmpty(lines[i]);

                if (prevLineEmpty == true && thisLineEmpty == true)
                {
                    ArrayUtility.RemoveAt(ref lines, i);
                    i--;
                }
            }

            if (string.IsNullOrEmpty(lines[0]) == false)
            {
                ArrayUtility.Insert(ref lines, 0, "");
            }

            while (lines.Length > 0 && string.IsNullOrEmpty(lines[lines.Length - 1]) == true)
            {
                ArrayUtility.RemoveAt(ref lines, lines.Length - 1);
            }

            char[] space = new char[] { ' ' };
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd(space);
            }

            AnalyzeNamespace(filePath, ref lines);
        }

        private static void AnalyzeNamespace(string filePath, ref string[] lines)
        {
            string expectedNamespace = GetNamespaceFromPath(filePath);
            uint matchCounter = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith("namespace"))
                {
                    lines[i] = $"namespace {expectedNamespace}";
                    matchCounter++;
                    if (i == 0 || string.IsNullOrEmpty(lines[i - 1]) == false)
                    {
                        ArrayUtility.Insert(ref lines, i, "");
                        i++;
                    }
                }
            }
            if (matchCounter != 1)
            {
                string msg = $"namespace missing ({expectedNamespace})";
                PrintError("NamespaceMissing", msg, filePath, null);
            }
        }

        private static void PrintStatistic(Dictionary<string, FolderStatistic> statisticPerFolder)
        {
            int totalLinesCount = statisticPerFolder[rootFolder].LinesCount;

            foreach (KeyValuePair<string, FolderStatistic> folderStatistic in statisticPerFolder)
            {
                string folder = folderStatistic.Key;
                if (folder != rootFolder)
                {
                    int filesCount = folderStatistic.Value.FilesCount;
                    int linesCount = folderStatistic.Value.LinesCount;
                    float linesCountPerc = 100f * (float)linesCount / (float)totalLinesCount;
                    Debug.Log($"[{green}Statistic{colorend}] [{yellow}{folder}{colorend}] {yellow}{linesCount}{colorend} lines in {yellow}{filesCount}{colorend} files ({yellow}{linesCountPerc:0.0}%{colorend})");
                }
            }
        }

        private static void PrintError(string type, string message, string pathToFile, uint? lineNumber)
        {
            Debug.LogError($"{yellow}{pathToFile.Replace('\\', '/')}:{lineNumber}{colorend} [{green}{type}{colorend}] -> {message}");
        }

        private static IEnumerable<string> GetAllCSharpFiles(string rootFolder)
        {
            foreach (string file in Directory.GetFiles(rootFolder))
            {
                if (file.ToLower().EndsWith(".cs"))
                {
                    yield return file;
                }
            }
            foreach (string folder in Directory.GetDirectories(rootFolder))
            {
                foreach (string file in GetAllCSharpFiles(folder))
                {
                    yield return file;
                }
            }
        }

        private static string GetNamespaceFromPath(string filePath)
        {
            string result = rootNamespace;
            int level = 2;
            while (true)
            {
                string nmPart = GetFolderFromPath(filePath, level);
                if (string.IsNullOrEmpty(nmPart) == true)
                {
                    return result;
                }
                else
                {
                    result += $".{nmPart}";
                    level++;
                }
            }
        }

        private static readonly char[] pathSeparators = new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar };
        private static string GetFolderFromPath(string filePath, int folderLevel)
        {
            string[] path = Path.GetDirectoryName(filePath).Split(pathSeparators);
            if (path.Length > folderLevel)
            {
                return path[folderLevel];
            }
            return string.Empty;
        }
    }
}
