using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager
{
    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] veinDatas;
    
    private Sprite[] beltSprites;

    private readonly int veinOffset = 11;
    private readonly int buildingOffset = 101;
    private readonly int itemOffset = 16;

    public void Init()
    {

        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        veinDatas = Resources.LoadAll<VeinData>("Data/VeinData");
        beltSprites = Resources.LoadAll<Sprite>("Sprites/Belts");
    }



    public ItemData GetItemData(int id)
    {
        return itemDatas[id-itemOffset];
    }

    public BuildingData GetBuildingData(int id)
    {
        return buildingDatas[id - buildingOffset];
    }

    public VeinData GetVeinData(int id)
    {
        return veinDatas[id - veinOffset];
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
