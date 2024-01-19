using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : BuildingBase
{
    [SerializeField]
    protected Belt nextBelt;

    public override void SetBeltId(int id, int rot = 0)
    {
        return;
    }
    public override bool IsTransferAble(int id, int rot)
    {
        return false;
    }

    public override void EraseNextBelt(int rot)
    {
        nextBelt = null;
    }


}
