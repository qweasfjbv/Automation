using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Automation/Vein")]
public class VeinData : ScriptableObject
{
    [SerializeField] private int veinID;
    [SerializeField] private int oreID;
    [SerializeField] private Sprite veinSprite;
    [SerializeField] private float miningTime;

    public int VeinID { get => VeinID; }
    public int OreID { get => oreID; }
    public Sprite VeinImage { get => veinSprite; }
    public float MiningTime { get => miningTime; }

}
