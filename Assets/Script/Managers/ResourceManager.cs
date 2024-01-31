using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ItemJsonData
{
    public int ID;
    public string Name;
    public List<Ingredient> Ingredient;
    public float ProductTime;
    public string Description;
    public int MakingBuildingId;
}
[Serializable]
public class ItemJsonDataArr
{
    public ItemJsonData[] itemJsonDatas;
}

[Serializable]
public class BuildingJsonData : ItemJsonData {
    public Vector2 Size;
    public float Speed;
    public List<int> InputDirs;
    public List<int> OutputDirs;
    public List<int> OutputIds;
}

[Serializable]
public class BuildingJsonDataArr {
    public BuildingJsonData[] buildingJsonDatas;
}


public class ResourceManager
{


    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] terrainDatas;
    

    public readonly int VEINOFFSET = 1;
    public readonly int BUILDINGOFFSET = 101;
    public readonly int ITEMOFFSET = 11;

    private ItemJsonDataArr tmpItemDatas;
    private BuildingJsonDataArr tmpBuildingDatas;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/JsonData/ItemData");
        tmpItemDatas = JsonUtility.FromJson<ItemJsonDataArr>(textAsset.text);
        tmpBuildingDatas = JsonUtility.FromJson<BuildingJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/BuildingData").text);


        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");

        for (int i = 0; i < tmpItemDatas.itemJsonDatas.Length; i++)
        {
            itemDatas[i].SetItemData(tmpItemDatas.itemJsonDatas[i]);
        }

        for (int i = 0; i < tmpBuildingDatas.buildingJsonDatas.Length; i++)
        {
            buildingDatas[i].SetBuildingData(tmpBuildingDatas.buildingJsonDatas[i]);
        }
        

        terrainDatas = Resources.LoadAll<VeinData>("Data/TerrainData");

    }

    public ItemData GetItemData(int id)
    {

        if (id < 100)
        {
            return itemDatas[id - ITEMOFFSET];
        }
        else
        {
            return GetBuildingData(id);
        }
    }

    public int GetItemCount()
    {
        return itemDatas.Length;
    }

    public int GetBuildingCount()
    {
        return buildingDatas.Length;
    }
    public BuildingData GetBuildingData(int id)
    {
        return buildingDatas[id - BUILDINGOFFSET];
    }

    public VeinData GetTerrainData(int id)
    {
        return terrainDatas[id - VEINOFFSET];
    }

    public Sprite GetBuildingSprite(int id)
    {
        return GetBuildingData(id).Image;
    }

    public Sprite GetItemSprite(int id)
    {
        return GetItemData(id).Image;
    }


}
