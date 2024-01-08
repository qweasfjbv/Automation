using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private ItemData[] itemDatas;
    private Sprite[] beltSprites; 

    public void Init()
    {
        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
        beltSprites = Resources.LoadAll<Sprite>("Sprites/Belts");
    }

    public ItemData[] ItemDatas { get => itemDatas; }
    public Sprite[] BeltSprites { get => beltSprites; }

}
