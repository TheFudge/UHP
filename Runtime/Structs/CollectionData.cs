using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    [System.Serializable]
    public class CollectionData
    {
        public string Identifier;
        #if UNITY_EDITOR
        public int TextureBytes, TextureCount, Batches, DrawCalls, Triangles, Vertices, SetPassCalls;
        #endif
        public string App, Scene, Platform, Device, Version;
        public double CPUFPS, GPUFPS;
        public bool isEditor = false;

        public void CollectAt(string identifier)
        {
            Identifier = identifier;
#if UNITY_EDITOR
            isEditor = true;
            DrawCalls = UnityStats.drawCalls;
            Vertices = UnityStats.vertices;
            Triangles = UnityStats.triangles;
            Batches = UnityStats.batches;
            TextureCount = UnityStats.renderTextureCount;
            TextureBytes = UnityStats.renderTextureBytes;
            SetPassCalls = UnityStats.setPassCalls;

#endif
            App = Application.productName;
            Scene = SceneManager.GetActiveScene().name;
            Version = Application.version;
            
            Platform = Application.platform.ToString();
            Device = SystemInfo.deviceModel;
                
            FrameTimingManager.CaptureFrameTimings();
            FrameTiming[] timing = new[] {new FrameTiming(),};
            FrameTimingManager.GetLatestTimings(1, timing);
            
            CPUFPS = timing[0].cpuFrameTime;
            GPUFPS = timing[0].gpuFrameTime;
        }
    }
}