using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{ 

    public Action<int> SetQuestUI { get; set; }
    public Action<int> SetQuestUIA { get; set; }
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
        Managers.Data.QuestProgress.inProgressId = id;
        SetQuestUI(id);
    }

    public void SetQuestIdAfterClear(int id)
    {
        Managers.Data.QuestProgress.inProgressId = id;
        SetQuestUIA(id);
    }

    public void OnSuccess()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.SUCCESS);
        Managers.Data.QuestProgress.inProgressId = -1;
        Managers.Data.QuestProgress.successId++;
    }


}
