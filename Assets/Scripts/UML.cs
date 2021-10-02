
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace Tank
{
    public static class LibraryReferencesUmlGenerator
    {
        [Serializable]
        private struct AsmDefStructure
        {
            public string name;
            public string[] references;
        }

        static string[] sharedLibraries = new string[]
        {
            "Unity.TextMeshPro",
            "Interfaces",
            "Helpers",
            "Configs",
        };

        [MenuItem("P504/Generate UML with library references")]
        static void GenerateUML()
        {
            string umlDocument = "";

            umlDocument += "@startuml" + System.Environment.NewLine;

            umlDocument += "scale max 1920*1080" + System.Environment.NewLine;

            ParseDirectory(Application.dataPath, ref umlDocument);

            umlDocument += "@enduml" + System.Environment.NewLine;

            string pathToFile = Path.Combine(Application.dataPath, "../architecture.plantuml");

            File.WriteAllText(pathToFile, umlDocument);

            Process.Start("chrome.exe", "file://" + pathToFile);
        }

        static void ParseDirectory(string directoryPath, ref string umlDocument)
        {
            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                if (filePath.EndsWith(".asmdef"))
                {
                    AsmDefStructure asmDef = JsonUtility.FromJson<AsmDefStructure>(File.ReadAllText(filePath));

                    umlDocument +=
                        "class " +
                        asmDef.name +
                        (IsSharedLibrary(asmDef.name) ? " #adff2f/fafad2" : " #2fadff/fafad2") +
                        " {" +
                        System.Environment.NewLine;

                    ReadClasses(asmDef.name, ref umlDocument);

                    umlDocument += "}" + System.Environment.NewLine;

                    umlDocument += System.Environment.NewLine;

                    if (asmDef.references != null)
                    {
                        foreach (string reference in asmDef.references)
                        {
                            if (IsSharedLibrary(reference) == false || IsSharedLibrary(asmDef.name) == true)
                            {
                                umlDocument += reference + " <-- " + asmDef.name + System.Environment.NewLine;
                            }
                        }
                    }

                    umlDocument += System.Environment.NewLine;
                }
            }

            foreach (string subdirectoryPath in Directory.GetDirectories(directoryPath))
            {
                ParseDirectory(subdirectoryPath, ref umlDocument);
            }
        }

        static bool IsSharedLibrary(string libraryName)
        {
            return Array.Exists(sharedLibraries, x => x == libraryName);
        }

        static void ReadClasses(string assemblyName, ref string umlDocument)
        {
            //foreach (Assembly A in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (string.Equals(A.GetName().Name, assemblyName))
            //    {
            //        foreach (Type type in A.GetTypes())
            //        {
            //            umlDocument += "    " + type.Name + Environment.NewLine;
            //        }
            //    }
            //}
        }
    }
}
