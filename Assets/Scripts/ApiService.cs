using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class ApiService {
    private const string URL = "http://91.188.125.24:8888/api/word/";
    private static List<Dictionary<string, string>> fetchedData = new List<Dictionary<string, string>>();

    public static async Task<bool> FetchData() {
        try {
            var request = (HttpWebRequest) WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse) await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream ?? Stream.Null)) {
                var data = await reader.ReadToEndAsync();
                fetchedData = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                //GameManager.AddWordsToDictionary(fetchedData);
                return true;
            }
        }
        catch (WebException e) {
            if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null) {
                var response = (HttpWebResponse) e.Response;
                if (response.StatusCode == HttpStatusCode.NotFound) return false;
            }
        }

        return false;
    }

    public static void SaveJsonFile(string path) {
        using (var file = File.CreateText(path)) {
            var serializer = new JsonSerializer();
            serializer.Serialize(file, fetchedData);
        }
    }
}