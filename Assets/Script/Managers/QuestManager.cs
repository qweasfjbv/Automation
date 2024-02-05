using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{ 

    public Action<int> SetQuestUI { get; set; }
    public Action QuestFail { get; set; }
    public Action QuestSuccess { get; set; }


    public void Init()
    {

        QuestFail -= (() => Managers.Data.QuestProgress.inProgressId = -1);
        QuestFail += (() => Managers.Data.QuestProgress.inProgressId = -1);

        QuestSuccess -= OnSuccess;
        QuestSuccess += OnSuccess;

    }

    public void SetQuestId(int id)
    {
        Managers.Data.QuestProgress.remainTimer = -1;
        Managers.Data.QuestProgress.inProgressId = id;
        SetQuestUI(id);
    }

    public void OnSuccess()
    {
        Managers.Data.QuestProgress.inProgressId = -1;
        Managers.Data.QuestProgress.successId++;
    }


}
