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
}
[Serializable]
public class ItemJsonDataArr
{
    public ItemJsonData[] itemJsonDatas;
}
public class ResourceManager
{


    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] veinDatas;
    

    private readonly int VEINOFFSET = 1;
    private readonly int BUILDINGOFFSET = 101;
    private readonly int ITEMOFFSET = 11;

    private ItemJsonDataArr tmpDatas;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/JsonData/ItemData");
        tmpDatas = JsonUtility.FromJson<ItemJsonDataArr>(textAsset.text);

        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");

        for (int i = 0; i < tmpDatas.itemJsonDatas.Length; i++)
        {
            itemDatas[i].SetItemData(tmpDatas.itemJsonDatas[i]);
        }

        

        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        veinDatas = Resources.LoadAll<VeinData>("Data/VeinData");

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

    public BuildingData GetBuildingData(int id)
    {
        return buildingDatas[id - BUILDINGOFFSET];
    }

    public VeinData GetVeinData(int id)
    {
        return veinDatas[id - VEINOFFSET];
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
