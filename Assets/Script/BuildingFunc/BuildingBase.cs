using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{

    protected int beltItemId;

    abstract public void SetBeltId(int id, int rot = 0);
    abstract public bool IsTransferAble(int rot);

}
