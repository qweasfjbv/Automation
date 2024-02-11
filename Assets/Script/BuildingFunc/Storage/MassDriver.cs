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
        return true;
    }

    public override void SetBeltId(int id, int rot = 0)
    {
        if (Managers.Data.QuestProgress.inProgressId != -1)
        {
            // 아직 미성공
            if(Managers.Data.QuestProgress.successId < Managers.Data.QuestProgress.inProgressId)
            {
                Managers.Data.AddQuestItem(id, 1);
            }
            else
            {
                // 우주선이면
                if(id == 39)
                {
                    Managers.Data.AddQuestPopulation(Managers.Data.QuestProgress.inProgressId, GameManagerEx.Instance.PEOPLEPERSHIP);
                }
                else
                {
                    GameManagerEx.Instance.ThrowItemsToSpace(id);
                }
            }
        }
        else
        {
            GameManagerEx.Instance.ThrowItemsToSpace(id);
        }
    }
}
