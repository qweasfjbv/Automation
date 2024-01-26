using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TileData {
    public int x, y;
    public int id;
    public int rot;
    public int terrainInfo;

    public int itemId;
    public int outDir;
}

[Serializable]
public class TileDatas {
    public List<TileData> tileData;
}



public class DataManager
{

    private TileDatas tileDatas;
    private string path;
    private string fileName = "/UserMapData.json";


    public void Init()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();


        path = Application.persistentDataPath;

        if (Managers.Scene.CurScene.GetComponent<GameScene>() != null)
        {
            LoadMap();
        }
    }

    // save on UserMapData.json
    // posx, posy, buildingid, rot,
    // if belt -> outdir, itemIdOnBelt

    public void SaveMap()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();

        for (int i = 0; i < Managers.Map.MapSizeX; i++)
        {
            for (int j = 0; j < Managers.Map.MapSizeY; j++)
            {
                Tile tmp = Managers.Map.GetTileOnPoint(new Vector2(i, j));

                if (tmp == null) continue;
                if (tmp.id == -1 && tmp.terrainInfo == 0) continue;

                if (tmp.id == 101)
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = tmp.building.GetComponent<Belt>().BeltItemId,
                        outDir = tmp.building.GetComponent<Belt>().OutDir
                    });
                }
                else if (tmp.building != null && tmp.building.GetComponent<Production>() != null)
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = tmp.building.GetComponent<Production>().OutputItemId,
                        outDir = 0
                    });
                }
                else
                {
                    tileDatas.tileData.Add(new TileData
                    {
                        x = i,
                        y = j,
                        id = tmp.id,
                        rot = tmp.rot,
                        terrainInfo = tmp.terrainInfo,
                        itemId = -1,
                        outDir = 0
                    }) ;
                }

            }
        }

        if (File.Exists(path + fileName))
        {
            File.Delete(path + fileName);
        }

        File.AppendAllText(path + fileName, JsonUtility.ToJson(tileDatas));

        tileDatas.tileData.Clear();

    }

    public bool LoadMap()
    {
        if(!File.Exists(path+fileName)) {
            return false;
        }
        tileDatas = JsonUtility.FromJson<TileDatas>(File.ReadAllText(path + fileName));

        if (tileDatas == null || tileDatas.tileData == null) return false;


        for (int i = 0; i < tileDatas.tileData.Count; i++)
        {
            var tile = Managers.Map.GetTileOnPoint(new Vector2(tileDatas.tileData[i].x, tileDatas.tileData[i].y));

            tile.terrainInfo = tileDatas.tileData[i].terrainInfo;


            if (tileDatas.tileData[i].id != -1)
            {
                Managers.Map.Build(tileDatas.tileData[i].id, new Vector2(tileDatas.tileData[i].x, -tileDatas.tileData[i].y),
                    new Vector2(1, 1), tileDatas.tileData[i].rot);
            }
        }

        for (int i = 0; i < tileDatas.tileData.Count; i++)
        {
            var tile = Managers.Map.GetTileOnPoint(new Vector2(tileDatas.tileData[i].x, tileDatas.tileData[i].y));

            if (tile.building == null) continue;

            if (tile.building.GetComponent<Belt>() != null)
            {
                tile.building.GetComponent<Belt>().BeltItemId = tileDatas.tileData[i].itemId;
                tile.building.GetComponent<Belt>().SetOutdir(tileDatas.tileData[i].outDir);
            }
            else if (tile.building.GetComponent<Production>() != null)
            {
                tile.building.GetComponent<Production>().OutputItemId = tileDatas.tileData[i].itemId;
            }
        }

        tileDatas.tileData.Clear();

        return true;
    }

}
