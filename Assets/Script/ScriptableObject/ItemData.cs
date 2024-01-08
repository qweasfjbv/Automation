using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Automation/Item")]
public class ItemData : ScriptableObject
{
    // <= 100 : resource, > 100 : building
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    
    
    [SerializeField] private float productTime;
    [SerializeField] private List<Vector2Int> ingredients = new List<Vector2Int>();


    
    
    
    
    
    public int ID { get => itemId; }
    public string Name { get => itemName; }
    public Sprite Image { get => sprite; } 
    public float ProductTime { get => productTime;}
    public List<Vector2Int> Ingredients { get => ingredients; }

}
