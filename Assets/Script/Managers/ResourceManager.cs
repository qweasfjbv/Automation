using UnityEngine;

public class ResourceManager
{
    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private Sprite[] beltSprites;

    public void Init()
    {
        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        beltSprites = Resources.LoadAll<Sprite>("Sprites/Belts");
    }

    public ItemData[] ItemDatas { get => itemDatas; }



    public ItemData GetItemData(int id)
    {
        return itemDatas[id];
    }

    public BuildingData GetBuildingData(int id)
    {
        return buildingDatas[id - 101];
    }

    public Sprite GetBeltSprite(int id)
    {
        // 0: straight, 1: turn right, 2: turn left;
        return beltSprites[id];
    }
    public Sprite GetBuildingSprite(int id)
    {
        // start from id:102, 101 is belt;
        return GetBuildingData(id).Image;
    }

    public Sprite GetItemSprite(int id)
    {
        return GetItemData(id).Image;
    }


}
