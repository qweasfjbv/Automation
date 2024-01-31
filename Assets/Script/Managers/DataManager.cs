using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private string mapFileName = "/UserMapData.json";
    private string invenItemFileName = "/UserInvenItemData.json";
    private string invenBuildingFileName = "/UserInvenBuildingData.json";

    private List<int> invenItemData;
    private List<int> invenBuildingData;
    public void Init()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();


        path = Application.persistentDataPath;

        if (Managers.Scene.CurScene.GetComponent<GameScene>() != null)
        {
            LoadMap();
        }
        else if (Managers.Scene.CurScene.GetComponent<TutorialScene>() != null)
        {
            LoadTutorialMap();
        }
    }

    public void SaveAll()
    {
        SaveMap();
        SaveInven();
    }

    private void LoadTutorialMap()
    {
        Managers.Map.GenerateTutorialMap();
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

        DeleteMap();

        File.AppendAllText(path + mapFileName, JsonUtility.ToJson(tileDatas));

        tileDatas.tileData.Clear();

    }

    public bool LoadMap()
    {


        if(!File.Exists(path+mapFileName)) {
            Managers.Map.GenerateVeinsOnMap();
            return false;
        }
        tileDatas = JsonUtility.FromJson<TileDatas>(File.ReadAllText(path + mapFileName));

        if (tileDatas == null || tileDatas.tileData == null) return false;


        for (int i = 0; i < tileDatas.tileData.Count; i++)
        {
            var tile = Managers.Map.GetTileOnPoint(new Vector2(tileDatas.tileData[i].x, tileDatas.tileData[i].y));

            tile.terrainInfo = tileDatas.tileData[i].terrainInfo;

            if(tile.terrainInfo >= 1 && tile.terrainInfo <= 7)
            {
                Managers.Map.BuildVein(tileDatas.tileData[i].y, tileDatas.tileData[i].x, tile.terrainInfo);
            }

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
                tile.building.GetComponent<Belt>().SetBeltId(tileDatas.tileData[i].itemId);
                tile.building.GetComponent<Belt>().SetOutdir(tileDatas.tileData[i].outDir);
            }
            else if (tile.building.GetComponent<Production>() != null)
            {
                tile.building.GetComponent<Production>().Init(tileDatas.tileData[i].itemId);
            }
        }

        tileDatas.tileData.Clear();

        return true;
    }

    public void DeleteMap()
    {
        if (File.Exists(path + mapFileName))
        {
            File.Delete(path + mapFileName);
        }
    }

    public ref List<int> LoadInvenItem() {

        if (!File.Exists(path + invenItemFileName))
        {
            invenItemData = new List<int>();
            for (int i = 0; i < Managers.Resource.GetItemCount(); i++)
            {
                invenItemData.Add(0);
            }

        }
        else
        {
            Debug.Log("Data Empty. Inven Save/Load Needed");
            //load
        }

        return ref invenItemData;
    }
    public ref List<int> LoadInvenBuilding()
    {

        if (!File.Exists(path + invenBuildingFileName))
        {
            invenBuildingData = new List<int>();
            for (int i = 0; i < Managers.Resource.GetBuildingCount(); i++)
            {
                invenBuildingData.Add(0);
            }

        }
        else
        {
            Debug.Log("Data Empty. Inven Save/Load Needed");
            //load
        }

        return ref invenBuildingData;
    }
    public void SaveInven()
    {

    }

}
