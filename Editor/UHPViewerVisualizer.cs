using System;
using System.Collections.Generic;
using UnityEngine;

namespace UHP.Editor
{
    public class UHPViewerVisualizer : MonoBehaviour
    {
        public  UHPViewerData Data = null;

        public float Scale = 1;

        public enum ValueType
        {
            Minimum, Maximum, Average
        }
        
        public enum MetricType
        {
            Triangles, Vertices, Batches, CPUFPS, GPUFPS, TextureCount, TextureBytes, SetPassCalls
        }

        public Gradient Color;
        public ValueType Value = ValueType.Average;
        public MetricType Metric = MetricType.CPUFPS;

        public float MinValue = 0f, MaxValue = 0f;
        
        public UHPViewerPoint Min, Max;
        
        private void OnDrawGizmos()
        {
            if (Scale < 0.01f)
            {
                Scale = 0.01f;
            }
            if (Data == null)
            {
                return;
            }

            bool first = true;
            
            foreach (KeyValuePair<Vector3, UHPViewerPoint> pointpair in Data.Points)
            {
                if (first)
                {
                    Min = pointpair.Value;
                    Max = pointpair.Value;
                    first = false;
                }
                CalcMinMax(pointpair.Value);
            }
            
            
            
            MinValue = GetCorrectValue(GetEntry(Min));
            MaxValue = GetCorrectValue(GetEntry(Max));
            
            foreach (KeyValuePair<Vector3, UHPViewerPoint> pointpair in Data.Points)
            {
                DrawSingleElement(pointpair.Key, pointpair.Value, MinValue, MaxValue);
            }
        }

        void DrawSingleElement(Vector3 position, UHPViewerPoint pointData, float min, float max)
        {
            UHPViewerEntry entry = GetEntry(pointData);

            float value = GetCorrectValue(entry);

            value = (value - min) / (max - min);
            
            Gizmos.color = Color.Evaluate(value);
            
            value = Scale * value;
            
            Gizmos.DrawCube(position, new Vector3(1f, value, 1f));
        }

        void CalcMinMax(UHPViewerPoint other)
        {
            Min = GetMinimalValue(Min, other);
            Max = GetMaximalValue(Max, other);
        }

        UHPViewerPoint GetMinimalValue(UHPViewerPoint a, UHPViewerPoint b)
        {
            return new UHPViewerPoint()
            {
                Identifier = "",
                Batches = GetMinimalValueEntry(a.Batches, b.Batches),
                Triangles = GetMinimalValueEntry(a.Triangles, b.Triangles),
                Vertices = GetMinimalValueEntry(a.Vertices, b.Vertices),
                DrawCalls = GetMinimalValueEntry(a.DrawCalls, b.DrawCalls),
                TextureBytes = GetMinimalValueEntry(a.TextureBytes, b.TextureBytes),
                TextureCount = GetMinimalValueEntry(a.TextureCount, b.TextureCount),
                SetPassCalls = GetMinimalValueEntry(a.SetPassCalls, b.SetPassCalls),
                CPUFPS = GetMinimalValueEntry(a.CPUFPS, b.CPUFPS),
                GPUFPS = GetMinimalValueEntry(a.GPUFPS, b.GPUFPS)
            };
        }

        UHPViewerEntry GetMinimalValueEntry(UHPViewerEntry a, UHPViewerEntry b)
        {
            return new UHPViewerEntry()
            {
                Avg = Math.Min(a.Avg, b.Avg),
                Min = Math.Min(a.Min, b.Min),
                Max = Math.Min(a.Max, b.Max)
            };
        }
        
        UHPViewerPoint GetMaximalValue(UHPViewerPoint a, UHPViewerPoint b)
        {
            return new UHPViewerPoint()
            {
                Identifier = "",
                Batches = GetMaximalValueEntry(a.Batches, b.Batches),
                Triangles = GetMaximalValueEntry(a.Triangles, b.Triangles),
                Vertices = GetMaximalValueEntry(a.Vertices, b.Vertices),
                DrawCalls = GetMaximalValueEntry(a.DrawCalls, b.DrawCalls),
                TextureBytes = GetMaximalValueEntry(a.TextureBytes, b.TextureBytes),
                TextureCount = GetMaximalValueEntry(a.TextureCount, b.TextureCount),
                SetPassCalls = GetMaximalValueEntry(a.SetPassCalls, b.SetPassCalls),
                CPUFPS = GetMaximalValueEntry(a.CPUFPS, b.CPUFPS),
                GPUFPS = GetMaximalValueEntry(a.GPUFPS, b.GPUFPS)
            };
        }

        UHPViewerEntry GetMaximalValueEntry(UHPViewerEntry a, UHPViewerEntry b)
        {
            return new UHPViewerEntry()
            {
                Avg = Math.Max(a.Avg, b.Avg),
                Min = Math.Max(a.Min, b.Min),
                Max = Math.Max(a.Max, b.Max)
            };
        }

        UHPViewerEntry GetEntry(UHPViewerPoint pointData)
        {
            switch (Metric)
            {
                case MetricType.Triangles:
                    return pointData.Triangles;
                case MetricType.Vertices:
                    return pointData.Vertices;
                case MetricType.Batches:
                    return pointData.Batches;
                case MetricType.CPUFPS:
                    return pointData.CPUFPS;
                case MetricType.GPUFPS:
                    return pointData.GPUFPS;
                case MetricType.TextureCount:
                    return pointData.TextureCount;
                case MetricType.TextureBytes:
                    return pointData.TextureBytes;
                case MetricType.SetPassCalls:
                    return pointData.SetPassCalls;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        float GetCorrectValue(UHPViewerEntry entry)
        {
            switch (Value)
            {
                case ValueType.Minimum:
                    return entry.Min;
                case ValueType.Maximum:
                    return entry.Min;
                case ValueType.Average:
                    return entry.Min;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}