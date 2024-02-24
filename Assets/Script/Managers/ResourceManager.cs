using System;
using System.Collections.Generic;
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

[Serializable] public class UpgradeJsonDataArr
{
    public UpgradeJsonData[] beltUpgradeData;
    public UpgradeJsonData[] factoryUpgradeData;
    public UpgradeJsonData[] miningUpgradeData;
}
[Serializable] public class UpgradeJsonData
{
    public float speed;
    public int cost;
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

    private UpgradeJsonDataArr tmpUpgradeJsonDatas; 



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

        tmpUpgradeJsonDatas = JsonUtility.FromJson<UpgradeJsonDataArr>(
            Resources.Load<TextAsset>("Data/JsonData/UpgradeData").text);

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

    public float GetUpgradeValue(int id, int floor)
    {
        switch (id) {
            case 0:
                return tmpUpgradeJsonDatas.beltUpgradeData[floor].speed;
            case 1:
                return tmpUpgradeJsonDatas.factoryUpgradeData[floor].speed;
            case 2:
                return tmpUpgradeJsonDatas.miningUpgradeData[floor].speed;
            default:
                return -1;
        }
    }
    public int GetUpgradeCost(int id, int floor)
    {
        if (GetUpgradeFloorCnt(id) <= floor) return -1;

        switch (id)
        {
            case 0:
                return tmpUpgradeJsonDatas.beltUpgradeData[floor].cost;
            case 1:
                return tmpUpgradeJsonDatas.factoryUpgradeData[floor].cost;
            case 2:
                return tmpUpgradeJsonDatas.miningUpgradeData[floor].cost;
            default:
                return -1;
        }
    }

    public int GetUpgradeFloorCnt(int id)
    {
        switch (id)
        {
            case 0:
                return tmpUpgradeJsonDatas.beltUpgradeData.Length;
            case 1:
                return tmpUpgradeJsonDatas.factoryUpgradeData.Length;
            case 2:
                return tmpUpgradeJsonDatas.miningUpgradeData.Length;
            default:
                return -1;
        }
    }
    // 0은 벨트 1은 공장, 2는 채굴
    public float GetCurBuildingSpeed(int id)
    {
        if (Managers.Data.GetUpgradeFloor(id) == 0)
        {
            Debug.Log("??");
            return 1;
        }

        return GetUpgradeValue(id, Managers.Data.GetUpgradeFloor(id)-1);
    }
}
