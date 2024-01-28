using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Automation/Building")]
public class BuildingData : ItemData
{
    [SerializeField] private Vector2 size;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;
    [SerializeField] private List<int> inputDirs;
    [SerializeField] private List<int> outputDirs;
    [SerializeField] private List<int> outputIds;


    public Vector2 Size { get => size; }
    public GameObject Prefab { get => prefab; }
    public float Speed { get => speed;}
    public List<int> InputDirs { get => inputDirs; }
    public List<int> OutputDirs { get => outputDirs; }
    public List<int> OutputIds { get => outputIds; }

    public void SetBuildingData(BuildingJsonData data)
    {
        SetItemData(data);

        size = data.Size;
        speed = data.Speed;
        inputDirs = data.InputDirs;
        outputDirs = data.OutputDirs;
        outputIds = data.OutputIds;

    }
}
