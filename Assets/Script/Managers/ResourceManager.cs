using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager
{
    private ItemData[] itemDatas;
    private BuildingData[] buildingDatas;
    private VeinData[] veinDatas;
    

    private readonly int VEINOFFSET = 1;
    private readonly int BUILDINGOFFSET = 101;
    private readonly int ITEMOFFSET = 11;

    public void Init()
    {

        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        buildingDatas = Resources.LoadAll<BuildingData>("Data/BuildingData");
        veinDatas = Resources.LoadAll<VeinData>("Data/VeinData");

    }



    public ItemData GetItemData(int id)
    {
        return itemDatas[id-ITEMOFFSET];
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
