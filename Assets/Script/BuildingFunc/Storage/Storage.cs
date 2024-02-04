using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : BuildingBase
{

    public override void EraseNextBelt(int rot)
    {
        ;
    }

    public override bool IsTransferAble(int id, int rot)
    {
        return true;
    }

    public override void SetBeltId(int id, int rot = 0)
    {
        Managers.Data.AddInvenItem(id, 1);
    }
}
