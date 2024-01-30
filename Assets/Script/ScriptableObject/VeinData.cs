using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Automation/Vein")]
public class VeinData : ScriptableObject
{
    [SerializeField] private int veinID;
    [SerializeField] private int oreID;
    [SerializeField] private Sprite veinSprite;
    [SerializeField] private GameObject prefab;

    public int VeinID { get => VeinID; }
    public int OreID { get => oreID; }
    public Sprite VeinImage { get => veinSprite; }
    public GameObject Prefab { get => prefab; } 

}
