using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Automation/Building")]
public class BuildingData : ItemData
{
    [SerializeField] private Vector2 size;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;
    [SerializeField] private List<int> inputs;

    public Vector2 Size { get => size; }
    public GameObject Prefab { get => prefab; }
    public float Speed { get => speed;}
    public List<int> Inputs { get => inputs; }
}
