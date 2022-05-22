using System;
using System.IO;
using UnityEngine;

public class UpdateDictionary : MonoBehaviour {
    private string path = "";
    public static Action OnDownloadingDone;

    private void Awake() {
        path = Path.Combine(Application.streamingAssetsPath, "spanish words.json");
        DownloadData();
    }

    private async void DownloadData() {
        if (await ApiService.FetchData()) {
            ApiService.SaveJsonFile(path);
        }

        OnDownloadingDone?.Invoke();
    }
}