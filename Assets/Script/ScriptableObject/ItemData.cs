using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    
    
    [SerializeField] private float productTime;
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();


    
    
    
    
    
    public int ID { get => itemId; }
    public string Name { get => itemName; }
    public Sprite Image { get => sprite; } 
    public float ProductTime { get => productTime;}
    public List<Ingredient> Ingredients { get => ingredients; }

}
