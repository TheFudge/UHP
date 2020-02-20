using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;

namespace UHP.Runtime
{
    public class StatsNetworkPush : StatsPushBehaviour
    {
        [SerializeField] private string URL;
        [SerializeField] private int dataCountForAPush = 10;
        [SerializeField] private int collectedData = 0;

        private Dictionary<string, string> data;

        private void Awake()
        {
            data = new Dictionary<string, string>();
        }

        public override void PushData(CollectionData newData)
        {
            data.Add("Data" + data.Count.ToString("00"), JsonUtility.ToJson(newData));
            collectedData = data.Count;
            if (data.Count >= dataCountForAPush)
            {
                PushToNetwork();
            }
        }

        private void OnDisable()
        {
            if (data.Count > 0)
            {
                PushToNetwork();
            }
        }

        private void PushToNetwork()
        {
            UnityWebRequest request = UnityWebRequest.Post(URL, data);
            Debug.Log("Sending "+data.Count+" stats data to " + URL);
            data.Clear();
            collectedData = 0;
            var asyncOperation = request.SendWebRequest();

            asyncOperation.completed += (a) =>
            {
                Debug.Log("Statistics push completed. " + request.error);
                Debug.Log(request.downloadHandler.text);
            };
        }
    }
}