using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UHP.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class UHPViewer : EditorWindow
{
    private UHPViewerInfo Info = null;

    private UHPViewerData Data = null;
    private Dictionary<string, List<string>> selections = new Dictionary<string, List<string>>();

    private int tab = 0;
    
    //private UHPViewData Data = null;

    private string URL = "127.0.0.1/UHP/";
    
    [MenuItem("Tools/UHP/Viewer")]
    private static void Create()
    {
        UHPViewer window = ScriptableObject.CreateInstance<UHPViewer>();
        window.Show();
        window.titleContent.text = "UHP Viewer";
    }

    private UHPViewerVisualizer visualizer = null;
    
    // Window has been selected
    void OnFocus() {
        // Remove delegate listener if it has previously
        // been assigned.
        visualizer = FindObjectOfType<UHPViewerVisualizer>();
        if (visualizer == null)
        {
            visualizer = new GameObject().AddComponent<UHPViewerVisualizer>();
            visualizer.name = DateTime.Now.ToString("f");
        }
    }
 
    void OnDestroy() {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        DestroyImmediate(visualizer);
        visualizer = null;
    }
 
    
    private void OnGUI()
    {
        
        if (Info == null)
        {
            ShowTabDownlad();
            return;
        }

        ShowTabs();

        switch (tab)
        {
            case 0:
                ShowTabViewer();
                break;
            case 1:
                ShowTabDownlad();
                break;
        }

    }

    void ShowTabs()
    {
        tab = GUILayout.Toolbar (tab, new string[] {"View", "Data"});
    }

    void ShowTabDownlad()
    {
        if (GUILayout.Button("Download Info"))
        {
            DownloadInfo();
        }
        
        GUILayout.BeginVertical();
        GUILayout.Label("App");
        DisplayApps();
        
        GUILayout.Space(8);
        GUILayout.Label("Scene");
        DisplayScenes();
        
        GUILayout.Space(8);
        GUILayout.Label("Platform");
        DisplayPlatforms();
        
        GUILayout.Space(8);
        GUILayout.Label("Devices");
        DisplayDevices();
        
        GUILayout.Space(8);
        GUILayout.Label("Devices");
        DisplayVersions();        
        GUILayout.EndVertical();

        if (GUILayout.Button("Download Data"))
        {
            DownloadData();
        }
        
        if (Data != null)
        {
            GUILayout.Label(Data.Points.Count.ToString("N0") + " points available");
        }
    }

    void ShowTabViewer()
    {
        if (Data == null)
        {
            return;
        }
        Editor objectEditor = Editor.CreateEditor(visualizer);
        objectEditor.OnInspectorGUI();
        foreach (var pair in Data.Points)
        {
            GUILayout.Label("Point: " + pair.Key.ToString() + " => " + pair.Value.Batches.Avg.ToString("N1"));
        }
    }
    
    
    void DisplayApps()
    {
        
        if (!selections.ContainsKey("App"))
        {
            selections.Add("App", new List<string>());
        }
        foreach (var app in Info.Apps)
        {
            bool active = selections["App"].Contains(app);
            
            bool newactive = GUILayout.Toggle(active, app);
            if (active == newactive)
            {
                continue;
            }
            Data = null;

            if (!newactive && selections["App"].Contains(app))
            {
                selections["App"].Remove(app);
            } else if (newactive && !selections["App"].Contains(app))
            {
                selections["App"].Add(app);
            }
        }
    }
    
    void DisplayScenes()
    {
        if (!selections.ContainsKey("Scene"))
        {
            selections.Add("Scene", new List<string>());
        }
        foreach (var scene in Info.Scenes)
        {
            bool active = selections["Scene"].Contains(scene);
            
            bool newactive = GUILayout.Toggle(active, scene);
            
            if (active == newactive)
            {
                continue;
            }
            Data = null;
            
            if (!newactive && selections["Scene"].Contains(scene))
            {
                selections["Scene"].Remove(scene);
            } else if (newactive && !selections["Scene"].Contains(scene))
            {
                selections["Scene"].Add(scene);
            }
        }
    }
    
    void DisplayPlatforms()
    {
        if (!selections.ContainsKey("Platform"))
        {
            selections.Add("Platform", new List<string>());
        }
        foreach (string platform in Info.Platforms)
        {
            bool active = selections["Platform"].Contains(platform);
            
            bool newactive = GUILayout.Toggle(active, platform);
            if (active == newactive)
            {
                continue;
            }
            Data = null;
            if (!newactive && selections["Platform"].Contains(platform))
            {
                selections["Platform"].Remove(platform);
            } else if (newactive && !selections["Platform"].Contains(platform))
            {
                selections["Platform"].Add(platform);
            }
        }
    }
    
    void DisplayVersions()
    {
        if (!selections.ContainsKey("Version"))
        {
            selections.Add("Version", new List<string>());
        }
        foreach (var version in Info.Versions)
        {
            bool active = selections["Version"].Contains(version);
            
            bool newactive = GUILayout.Toggle(active, version);
            if (active == newactive)
            {
                continue;
            }
            Data = null;
            if (!newactive && selections["Version"].Contains(version))
            {
                selections["Version"].Remove(version);
            } else if (newactive && !selections["Version"].Contains(version))
            {
                selections["Version"].Add(version);
            }
        }
    }
    
    void DisplayDevices()
    {
        if (!selections.ContainsKey("Device"))
        {
            selections.Add("Device", new List<string>());
        }
        foreach (var device in Info.Devices)
        {
            bool active = selections["Device"].Contains(device);
            
            bool newactive = GUILayout.Toggle(active, device);
            if (active == newactive)
            {
                continue;
            }
            Data = null;
            if (!newactive && selections["Device"].Contains(device))
            {
                selections["Device"].Remove(device);
            } else if (newactive && !selections["Device"].Contains(device))
            {
                selections["Device"].Add(device);
            }
        }
    }

    private void DownloadInfo()
    {
        EditorUtility.DisplayProgressBar("Download", "Downloading Info", 0f);
        try
        {
            DownloadInfoData();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        EditorUtility.ClearProgressBar();
    }


    private void DownloadInfoData()
    {
        string url = URL + "?action=info";
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        while (!request.isDone)
        {
            EditorUtility.DisplayProgressBar("Download", "Downloading Info: " + request.downloadProgress, request.downloadProgress);
            Thread.Sleep(10);
        }

        try
        {
            UHPViewerInfo info = JsonUtility.FromJson<UHPViewerInfo>(request.downloadHandler.text);
            Info = info;
            Debug.Log(request.downloadHandler.text);
        }
        catch (Exception ex)
        {
            EditorUtility.DisplayDialog("Error", "Info could not be parsed.", "Ok");
            Debug.LogException(ex);
            Debug.LogWarning(request.downloadHandler.text);
        }
    }
    
    private void DownloadData()
    {
        EditorUtility.DisplayProgressBar("Download", "Downloading Data", 0f);
        try
        {
            DownloadPointData();

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        EditorUtility.ClearProgressBar();
    }

    
    private void DownloadPointData()
    {
        string url = URL + "?action=get";
        Debug.Log(url);
        Dictionary<string, string> postdata = new Dictionary<string, string>();
        foreach (KeyValuePair<string, List<string>> selection in selections)
        {
            StringBuilder builder = new StringBuilder();
            bool first = true;
            foreach (string entry in selection.Value)
            {
                if (!first)
                {
                    builder.Append(";");
                }
                first = false;
                
                builder.Append(entry);
            }
            postdata.Add(selection.Key, builder.ToString());
        }
        
        UnityWebRequest request = UnityWebRequest.Post(url, postdata);
        request.SendWebRequest();
        while (!request.isDone)
        {
            EditorUtility.DisplayProgressBar("Download", "Downloading Info: " + request.downloadProgress, request.downloadProgress);
            Thread.Sleep(10);
        }

        try
        {
            Data =
                new UHPViewerData(JsonUtility.FromJson<UHPViewerSerializedData>(request.downloadHandler.text));
            
            visualizer.Data = Data;
        }
        catch (Exception ex)
        {
            EditorUtility.DisplayDialog("Error", "Info could not be parsed.", "Ok");
            Debug.LogException(ex);
            Debug.LogWarning(request.downloadHandler.text);
        }
    }

}