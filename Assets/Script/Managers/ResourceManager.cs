using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private ItemData[] itemDatas;

    public void Init()
    {
        itemDatas = Resources.LoadAll<ItemData>("Data/ItemData");
    }

    public ItemData[] ItemDatas { get => itemDatas; }

}
