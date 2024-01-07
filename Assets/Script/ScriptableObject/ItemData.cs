using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Automation/Item")]
public class ItemData : ScriptableObject
{
    // <= 100 : resource, > 100 : building
    [SerializeField] private int itemId;

    [SerializeField] private Sprite sprite;
    
    
    [SerializeField] private float productTime;
    [SerializeField] private int[,] ingredients = new int[2, 5];

    [SerializeField] private Vector2 size;

    [SerializeField] private GameObject prefab;
    
    
    
    
    
    public int ID { get => itemId; }
    public Sprite Image { get => sprite; } 
    public float ProductTime { get => productTime;}
    public int[,] Ingredients { get => ingredients; }
    public Vector2 Size { get => size; }
    public GameObject Prefab { get => prefab; }

}
