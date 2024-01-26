using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Production : BuildingBase
{
    [SerializeField]
    protected int outputItemId = -1;

    public int OutputItemId { get { return outputItemId; } set => outputItemId = value; }

    abstract public void ChangeOutputItemId(int id);
}
