using System.Collections.Generic;
using UnityEngine;

namespace UHP.Editor
{
    public class UHPViewerData
    {
        public Dictionary<Vector3, UHPViewerPoint> Points = new Dictionary<Vector3,UHPViewerPoint>();

        public UHPViewerPoint Max, Min = new UHPViewerPoint();
        
        public UHPViewerData(UHPViewerSerializedData serialized)
        {
            Points = new Dictionary<Vector3, UHPViewerPoint>();
            Max = new UHPViewerPoint();
            Min = new UHPViewerPoint();
            foreach (var entry in serialized.Points)
            {
                string[] split = entry.Identifier.Split(':');
                
                Vector3 vector = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
                
                Points.Add(vector, entry);

                
            }
        }
    }
    
    [System.Serializable]
    public class UHPViewerSerializedData
    {
        public UHPViewerPoint[] Points = new UHPViewerPoint[0];
    }
    
    [System.Serializable]
    public class UHPViewerPoint
    {
        public string Identifier;
        public UHPViewerEntry CPUFPS = new UHPViewerEntry(),
            GPUFPS = new UHPViewerEntry();

        public UHPViewerEntry DrawCalls = new UHPViewerEntry(),
            Triangles = new UHPViewerEntry(),
            Vertices = new UHPViewerEntry(),
            Batches = new UHPViewerEntry(),
            TextureCount = new UHPViewerEntry(),
            TextureBytes = new UHPViewerEntry(),
            SetPassCalls = new UHPViewerEntry();
    }
    
    [System.Serializable]
    public class UHPViewerEntry
    {
        public float Min, Avg, Max;
        public int Count;
    }
}