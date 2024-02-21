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
    public List<Ingredient> ingredients;
    public int populationLimit;
    public string questName;
    public string questDescription;
}
[Serializable] public class QuestJsonDataArr
{
    public QuestJsonData[] questJsonDatas;
}
[Serializable] public class HelpJsonData {
    public string[] helpJsonDatas;
}


public class ResourceManager
{


    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] terrainDatas;
    private QuestData[] questDatas;
    private HelpData[] helpDatas;
    
    public readonly static int VEINOFFSET = 1;
    public readonly static int BUILDINGOFFSET = 101;
    public readonly static int ITEMOFFSET = 11;

    private ItemJsonDataArr tmpItemDatas;
    private BuildingJsonDataArr tmpBuildingDatas;
    private QuestJsonDataArr tmpQuestDatas;
    private HelpJsonData tmpHelpDatas;

    public void Init()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/JsonData/ItemData");
        tmpItemDatas = JsonUtility.FromJson<ItemJsonDataArr>(textAsset.text);
        tmpBuildingDatas = JsonUtility.FromJson<BuildingJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/BuildingData").text);
        tmpQuestDatas = JsonUtility.FromJson<QuestJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/QuestData").text);
        tmpHelpDatas = JsonUtility.FromJson<HelpJsonData>(
            Resources.Load<TextAsset>("Data/JsonData/HelpData").text);


        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        questDatas = Resources.LoadAll<QuestData>("Data/QuestData");
        helpDatas = Resources.LoadAll<HelpData>("Data/HelpData");


        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items");
        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Sprites/Buildings");
        Sprite[] helpSprites = Resources.LoadAll<Sprite>("Sprites/Help");

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


        if (tmpQuestDatas.questJsonDatas != null)
        {
            for (int i = 0; i < tmpQuestDatas.questJsonDatas.Length; i++)
            {
                questDatas[i].SetQuestData(tmpQuestDatas.questJsonDatas[i]);
            }
        }

        for (int i = 0; i < helpDatas.Length; i++)
        {
            helpDatas[i].ID = i;
            helpDatas[i].HelpSprite = helpSprites[i];
            helpDatas[i].HelpContent = tmpHelpDatas.helpJsonDatas[i];
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

    public string GetHelpContent(int id)
    {
        return helpDatas[id].HelpContent;
    }
    public Sprite GetHelpSprite(int id)
    {
        return helpDatas[id].HelpSprite;
    }

}
