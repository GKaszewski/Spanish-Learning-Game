using System.Collections.Generic;

public class Word{
    public string English {get; set;}
    public string Spanish {get; set;}

    public Word(){

    }

    public Word(string english, string spanish){
        English = english;
        Spanish = spanish;
    }

    public Dictionary<string, string> ToDictionary()
    {
        var dictionary = new Dictionary<string, string>();
        dictionary.Add("Spanish", Spanish);
        dictionary.Add("English", English);
        return dictionary;
    }
}