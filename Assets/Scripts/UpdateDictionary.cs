using System;
using UnityEngine;

public class UpdateDictionary : MonoBehaviour {
    private string path = "";
    public static Action OnDownloadingDone;

    private void Awake() {
        path = $"{Application.dataPath}/StreamingAssets/spanish words.json";
        DownloadData();
    }

    private async void DownloadData() {
        if (await ApiService.FetchData()) {
            ApiService.SaveJsonFile(path);
        }

        OnDownloadingDone?.Invoke();
    }
}