using UnityEngine;
using UnityEditor;
using System.Collections.Generic;




public struct MapSave
{
    public MapSave(List<WallSave> walls, List<Vector2> spawns, string gameName)
    {
        Walls = walls;
        SpawnPoints = spawns;
        GameName = gameName;
    }
    public List<WallSave> Walls;
    public List<Vector2> SpawnPoints;
    public string GameName;
}

[System.Serializable]
public struct WallSave
{
    public WallSave(float PosXin, float PosYin, float SizeXin, float SizeYin)
    {
        PosX = PosXin;
        PosY = PosYin;
        SizeX = SizeXin;
        SizeY = SizeYin;
    }
    public float PosX;
    public float PosY;

    public float SizeX;
    public float SizeY;
}