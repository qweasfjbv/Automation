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

public class InvenItemDatas {
    public int[] invenItemData;
}
public class InvenBuildingDatas {
    public int[] invenBuildingData;
}


public class DataManager
{

    private TileDatas tileDatas;
    private string path;
    private string mapFileName = "/UserMapData.json";
    private string invenItemFileName = "/UserInvenItemData.json";
    private string invenBuildingFileName = "/UserInvenBuildingData.json";

    InvenItemDatas invenItemDatas = new InvenItemDatas();
    InvenBuildingDatas invenBuildingDatas = new InvenBuildingDatas();


    public void Init()
    {
        tileDatas = new TileDatas();
        tileDatas.tileData = new List<TileData>();


        path = Application.persistentDataPath;

        Debug.Log(path);
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

    public ref int[] LoadInvenItem() {

        if (!File.Exists(path + invenItemFileName))
        {

            invenItemDatas.invenItemData = new int[Managers.Resource.GetItemCount()];
            for (int i = 0; i < Managers.Resource.GetItemCount(); i++)
            {
                invenItemDatas.invenItemData[i] = 0;
            }

        }
        else
        {
            invenItemDatas = JsonUtility.FromJson<InvenItemDatas>(File.ReadAllText(path + invenItemFileName));
        }

        if (invenItemDatas == null) invenItemDatas = new InvenItemDatas();

        if (invenItemDatas.invenItemData == null || invenItemDatas.invenItemData.Length <= 0)
        {
            invenItemDatas.invenItemData = new int[Managers.Resource.GetItemCount()];
            for (int i = 0; i < Managers.Resource.GetItemCount(); i++)
            {
                invenItemDatas.invenItemData[i] = 0;
            }


        }

        return ref invenItemDatas.invenItemData;
    }
    public ref int[] LoadInvenBuilding()
    {

        if (!File.Exists(path + invenBuildingFileName))
        {

            invenBuildingDatas.invenBuildingData = new int[Managers.Resource.GetBuildingCount()];
            for (int i = 0; i < Managers.Resource.GetBuildingCount(); i++)
            {
                invenBuildingDatas.invenBuildingData[i] = 0;
            }

        }
        else
        {
            invenBuildingDatas = JsonUtility.FromJson<InvenBuildingDatas>(File.ReadAllText(path + invenBuildingFileName));
        }

        if (invenBuildingDatas == null) invenItemDatas = new InvenItemDatas();

        if (invenBuildingDatas.invenBuildingData == null || invenBuildingDatas.invenBuildingData.Length <= 0)
        {
            invenBuildingDatas.invenBuildingData = new int[Managers.Resource.GetBuildingCount()];
            for (int i = 0; i < Managers.Resource.GetBuildingCount(); i++)
            {
                invenBuildingDatas.invenBuildingData[i] = 0;
            }


        }

        return ref invenBuildingDatas.invenBuildingData;
    }
    public void SaveInven()
    {
        DeleteInvenData();

        File.AppendAllText(path + invenItemFileName, JsonUtility.ToJson(invenItemDatas));
        File.AppendAllText(path + invenBuildingFileName, JsonUtility.ToJson(invenBuildingDatas));
    }
    private void DeleteInvenData()
    {
        if (File.Exists(path + invenBuildingFileName))
        {
            File.Delete(path + invenBuildingFileName);
        }
        if (File.Exists(path + invenItemFileName))
        {
            File.Delete(path + invenItemFileName);
        }
    }

}
