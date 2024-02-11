using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable] public class ItemJsonData
{
    public int ID;
    public string Name;
    public List<Ingredient> Ingredient;
    public float ProductTime;
    public string Description;
    public int MakingBuildingId;
}
[Serializable] public class ItemJsonDataArr
{
    public ItemJsonData[] itemJsonDatas;
}
[Serializable] public class BuildingJsonData : ItemJsonData {
    public Vector2 Size;
    public float Speed;
    public List<int> InputDirs;
    public List<int> OutputDirs;
    public List<int> OutputIds;
}
[Serializable] public class BuildingJsonDataArr {
    public BuildingJsonData[] buildingJsonDatas;
}
[Serializable] public class QuestJsonData
{
    public int questId;
    public int questLv;
    public List<Ingredient> ingredients;
    public float timeLimit;
    public string questName;
}
[Serializable] public class QuestJsonDataArr
{
    public QuestJsonData[] data;
}

public class ResourceManager
{


    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] terrainDatas;
    private QuestData[] questDatas;

    public readonly static int VEINOFFSET = 1;
    public readonly static int BUILDINGOFFSET = 101;
    public readonly static int ITEMOFFSET = 11;

    private ItemJsonDataArr tmpItemDatas;
    private BuildingJsonDataArr tmpBuildingDatas;
    private QuestJsonDataArr tmpQuestDatas;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/JsonData/ItemData");
        tmpItemDatas = JsonUtility.FromJson<ItemJsonDataArr>(textAsset.text);
        tmpBuildingDatas = JsonUtility.FromJson<BuildingJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/BuildingData").text);
        tmpQuestDatas = JsonUtility.FromJson<QuestJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/QuestData").text);

        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        questDatas = Resources.LoadAll<QuestData>("Data/QuestData");

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items");
        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Sprites/Buildings");

        for (int i = 0; i < itemDatas.Length; i++)
        {
            itemDatas[i].Image = sprites[i];
        }

        for (int i = 0; i < buildingDatas.Length; i++)
        {
            buildingDatas[i].Image = buildingSprites[i];
        }

        for (int i = 0; i < tmpItemDatas.itemJsonDatas.Length; i++)
        {
            itemDatas[i].SetItemData(tmpItemDatas.itemJsonDatas[i]);
        }

        for (int i = 0; i < tmpBuildingDatas.buildingJsonDatas.Length; i++)
        {
            buildingDatas[i].SetBuildingData(tmpBuildingDatas.buildingJsonDatas[i]);
        }


        if (tmpQuestDatas.data != null)
        {
            for (int i = 0; i < tmpQuestDatas.data.Length; i++)
            {
                questDatas[i].SetQuestData(tmpQuestDatas.data[i]);
            }
        }

        terrainDatas = Resources.LoadAll<VeinData>("Data/TerrainData");

    }


    public int GetItemCount()
    {
        return itemDatas.Length;
    }
    public int GetBuildingCount()
    {
        return buildingDatas.Length;
    }
    public int GetQuestCount()
    {
        return questDatas.Length;
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

    public QuestData GetQuestData(int id)
    {
        return questDatas[id];
    }



}
