using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private int curQuestId = -1;
    public int CurQuestId { get => curQuestId; }

    public Action<int> SetQuestUI { get; set; }
    public Action QuestFail { get; set; }

    public void Init()
    {
        QuestFail -= (() => curQuestId = -1);
        QuestFail += (() => curQuestId = -1);
    }

    public void SetQuestId(int id)
    {
        curQuestId = id;
        SetQuestUI(id);
    }

    


}
