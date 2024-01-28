using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct Ingredient {
    public int id;
    public int cnt;
};


[CreateAssetMenu(menuName = "Automation/Item")]
public class ItemData : ScriptableObject
{
    // <= 100 : resource, > 100 : building
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string itemDescription;
    
    [SerializeField] private float productTime;
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] private int makingBuildingId;
    
    
    
    
    
    public int ID { get => itemId; }
    public string Name { get => itemName; }
    public Sprite Image { get => sprite; } 
    public float ProductTime { get => productTime;}
    public List<Ingredient> Ingredients { get => ingredients; }
    public string Description { get => itemDescription; }   
    public int MakingBuildingId { get => makingBuildingId; }

    public void SetItemData(ItemJsonData data)
    {
        itemId = data.ID;
        itemName = data.Name;
        ingredients.Clear();
        ingredients = data.Ingredient;
        productTime = data.ProductTime;
        itemDescription = data.Description;
        makingBuildingId = data.MakingBuildingId;
    }
}
