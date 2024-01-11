using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{

    protected int beltItemId;
    public int BeltItemId { get => beltItemId; set => beltItemId = value; }

    public BuildingBase nextBuilding;




}
