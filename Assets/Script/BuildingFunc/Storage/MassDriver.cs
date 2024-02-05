using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassDriver : BuildingBase
{

    public override void EraseNextBelt(int rot)
    {
        ;
    }

    public override bool IsTransferAble(int id, int rot)
    {
        if (Managers.Data.QuestProgress.inProgressId != -1)
        {
            return true;
        }
        else return false;
    }

    public override void SetBeltId(int id, int rot = 0)
    {
        Managers.Data.AddQuestItem(id, 1);
    }
}
