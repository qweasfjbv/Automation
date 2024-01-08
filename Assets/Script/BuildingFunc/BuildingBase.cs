using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    public enum Dir
    {
        UP = 0, RIGHT, DOWN, LEFT
    }

    private int buildingId;

    public abstract void SetDirs();

}
