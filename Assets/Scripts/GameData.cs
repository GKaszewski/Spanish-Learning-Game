using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable()]
public class GameData : ISerializable
{
    /* Data that you want to store */
    public int points;
    public GameMode gameMode = GameMode.SPANISH_TO_ENGLISH;
    public DifficultyLevel difficulty = DifficultyLevel.EASY;
    public GameData(){}

    public GameData(SerializationInfo info, StreamingContext context){
        points = (int) info.GetValue("points", typeof(int));
        difficulty = (DifficultyLevel) info.GetValue("difficulty", typeof(DifficultyLevel));
        gameMode = (GameMode) info.GetValue("gamemode", typeof(GameMode));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context){
        info.AddValue("points", points, typeof(int));
        info.AddValue("gamemode", gameMode, typeof(GameMode));
        info.AddValue("difficulty", difficulty, typeof(DifficultyLevel));
    }

}
