using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Net;
using System.IO;

/// <summary>
/// Editor tool to manage multiple remote .unitypackage downloads.
/// </summary>
public class UnityPackageDownloader : EditorWindow
{
    private struct RemotePackage
    {
        public string Name;
        public string Url;

        public RemotePackage(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }

    // Just name + URL now
    private static readonly List<RemotePackage> remotePackages = new List<RemotePackage>
    {
        new RemotePackage("My Utilities", "https://github.com/anuj-chouhan/My-Unity-Tools/releases/download/MyUtilities/My.Utilities.1.0.0.unitypackage"),
        // Add more here
    };

    [MenuItem("Tools/Import Remote Package")]
    public static void ShowWindow()
    {
        GetWindow<UnityPackageDownloader>("Import Remote Packages");
    }

    private Vector2 scrollPos;

    private void OnGUI()
    {
        GUILayout.Label("Available Remote UnityPackages", EditorStyles.boldLabel);
        GUILayout.Space(5);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var pkg in remotePackages)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Package:", pkg.Name, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("URL:", pkg.Url);

            if (GUILayout.Button("Download & Import"))
            {
                string fileName = GetFileNameFromUrl(pkg.Url);
                DownloadAndImportPackage(pkg.Url, fileName);
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Download & Import All Packages"))
        {
            foreach (var pkg in remotePackages)
            {
                string fileName = GetFileNameFromUrl(pkg.Url);
                DownloadAndImportPackage(pkg.Url, fileName);
            }
        }
        GUI.backgroundColor = Color.white;
    }

    private string GetFileNameFromUrl(string url)
    {
        return Path.GetFileName(url); // Extracts the file name from the URL
    }

    private void DownloadAndImportPackage(string url, string fileName)
    {
        string tempPath = Path.Combine(Path.GetTempPath(), fileName);
        Debug.Log("Downloading package from: " + url);

        try
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, tempPath);
            }

            Debug.Log("Download complete. Importing package...");
            Debug.Log("Downloaded file size: " + new FileInfo(tempPath).Length + " bytes");
            AssetDatabase.ImportPackage(tempPath, true); // true = show import dialog
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to download or import package: " + ex.Message);
        }
    }
}



//using UnityEngine;
//using UnityEditor;
//using System.Net;
//using System.IO;

//public class UnityPackageDownloader : EditorWindow
//{
//    private string packageUrl = "https://github.com/anuj-chouhan/My-Unity-Tools/releases/download/SaveLoadSystem/SaveLoadSystem.unitypackage";

//    private string saveFileName = "SaveLoadManager.unitypackage";

//    [MenuItem("Tools/Import Remote UnityPackage")]
//    public static void ShowWindow()
//    {
//        GetWindow<UnityPackageDownloader>("Import UnityPackage");
//    }

//    private void OnGUI()
//    {
//        GUILayout.Label("Download & Import .unitypackage", EditorStyles.boldLabel);

//        packageUrl = EditorGUILayout.TextField("Package URL:", packageUrl);
//        saveFileName = EditorGUILayout.TextField("Save As:", saveFileName);

//        if (GUILayout.Button("Download and Import"))
//        {
//            DownloadAndImportPackage(packageUrl, saveFileName);
//        }
//    }

//    private void DownloadAndImportPackage(string url, string fileName)
//    {
//        string tempPath = Path.Combine(Path.GetTempPath(), fileName);
//        Debug.Log("Downloading package from: " + url);

//        try
//        {
//            using (WebClient client = new WebClient())
//            {
//                client.DownloadFile(url, tempPath);
//            }

//            Debug.Log("Download complete. Importing package...");
//            Debug.Log("Downloaded file size: " + new FileInfo(tempPath).Length + " bytes");
//            AssetDatabase.ImportPackage(tempPath, true);  // true = show import dialog
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError("Failed to download or import package: " + ex.Message);
//        }
//    }
//}
