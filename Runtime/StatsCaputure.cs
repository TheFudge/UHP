using System.Diagnostics;
using DefaultNamespace;
using UnityEngine;

namespace UHP.Runtime
{
    public class StatsCaputure : MonoBehaviour
    {
        [SerializeField] private float CaptureTime = 1f;
        [SerializeField] private StatsPushBehaviour[] statsPush;
        
        [SerializeField] private CollectionData lastData;

        public bool CaptureOnIdChange = false;
        public bool CaptureOnTimer = false;
        public float RoundPositionTo = 0.1f;
        [SerializeField]
        private float timer = 0f;
        private Stopwatch gpuWatch, cpuWatch;
        [SerializeField]
        private double[] renderTimes, cpuTimes;
        private int renderTimesPosition = 0, cpuTimesPosition = 0;
        private string lastIdentifier = "";
        
        private void Start()
        {
            renderTimes = new double[15];
            cpuTimes = new double[15];
            
            lastData = new CollectionData();
            gpuWatch = new Stopwatch();
            cpuWatch = new Stopwatch();
            cpuWatch.Start();

            Camera.onPreRender += PreRender;
            Camera.onPostRender += PostRender;
        }

        private void PreRender(Camera cam)
        {
            gpuWatch.Reset();
            gpuWatch.Start();
        }

        private void PostRender(Camera cam)
        {
            gpuWatch.Stop();
            renderTimes[renderTimesPosition] = gpuWatch.Elapsed.TotalMilliseconds;
            renderTimesPosition = (renderTimesPosition + 1) % renderTimes.Length;
        }

        private void Update()
        {            
            cpuWatch.Stop();
            cpuTimes[cpuTimesPosition] = cpuWatch.Elapsed.TotalMilliseconds;
            cpuTimesPosition = (cpuTimesPosition + 1) % cpuTimes.Length;

            if (CaptureOnIdChange && lastIdentifier != GetIdentifier())
            {
                CaptureData();
                lastIdentifier = GetIdentifier();
            }

            if (CaptureOnTimer)
            {
                IncreaseTimer();

                if (CheckCaptureNow())
                {
                    CaptureData();
                }
            }
            
            cpuWatch.Reset();
            cpuWatch.Start();
        }

        private void IncreaseTimer()
        {
            timer += Time.deltaTime;
        }

        private bool CheckCaptureNow()
        {
            return timer >= CaptureTime;
        }

        private void CaptureData()
        {
            timer = Mathf.Max(0f, timer - CaptureTime);
            
            lastData.CollectAt(GetIdentifier());
            lastData.GPUFPS = GetFrameTimeCpu();
            lastData.CPUFPS = GetFrameTimeGpu();

            for (int i = 0; i < statsPush.Length; i++)
            {
                statsPush[i].PushData(lastData);
            }
        }

        private string GetIdentifier()
        {
            Vector3 position = transform.position;

            position.x = RoundPositionTo * Mathf.Round(position.x / RoundPositionTo);
            position.y = RoundPositionTo * Mathf.Round(position.y / RoundPositionTo);
            position.z = RoundPositionTo * Mathf.Round(position.z / RoundPositionTo);
            
            return
                position.x.ToString("0.000") + ":"+
                position.y.ToString("0.000") + ":"+
                position.z.ToString("0.000") + ":"
            ;
        }

        private double GetFrameTimeCpu()
        {
            double time = 0d;
            for (int i = 0; i < cpuTimes.Length; i++)
            {
                time += cpuTimes[i];
            }

            return time / (double) cpuTimes.Length;
        }
        
        private double GetFrameTimeGpu()
        {
            double time = 0d;
            for (int i = 0; i < renderTimes.Length; i++)
            {
                time += renderTimes[i];
            }

            return time / (double) renderTimes.Length;
        }
    }
}