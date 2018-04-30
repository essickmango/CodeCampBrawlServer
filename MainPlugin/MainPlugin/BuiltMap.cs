using UnityEngine;
using UnityEditor;
using System.Collections.Generic;




public struct MapSave
{
    public MapSave(List<WallSave> walls, List<Vector2> spawns, string mapName)
    {
        Walls = walls;
        SpawnPoints = spawns;
        MapName = mapName;
    }
    public List<WallSave> Walls;
    public List<Vector2> SpawnPoints;
    public string MapName;
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