using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Reflection;

public class VersionBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
       if(!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName)){
           Type type = null;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            type = Type.GetType($"{typeName}, {assemblyName}");
            return type;
       }

       return null;
    }
}

public class SaveSystem : MonoBehaviour
{
    public static string Path {get;set;} = "Data/Save1.sinq";

    public static void SaveData(GameData data){

        if(!Directory.Exists($"{Application.dataPath}/Data")){
            Directory.CreateDirectory($"{Application.dataPath}/Data");
        }
        
        Path = $"{Application.dataPath}/Data/Save1.sinq";

        using (var stream = new FileStream(Path, FileMode.Create, FileAccess.Write)){
            var bf = new BinaryFormatter();
            bf.Binder = new VersionBinder();
            bf.Serialize(stream, data);
            stream.Flush();
        }
    }

    public static GameData LoadData(){
        Path = $"{Application.dataPath}/Data/Save1.sinq";
        var gameDataToLoad = new GameData();

        if(!File.Exists(Path)){
            return null;
        }
        
        using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read)){
            var bf = new BinaryFormatter();
            bf.Binder = new VersionBinder();
            return (GameData) bf.Deserialize(stream);
        }
    }
}
