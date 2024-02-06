using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ProgressTime : MonoBehaviour
{
    private Coroutine progress;
    private bool isProgress = false;
    private TextMeshProUGUI timeText;

    public void SetProgTime(float t)
    {
        Managers.Data.QuestProgress.remainTimer = t;
        timeText = GetComponent<TextMeshProUGUI>();
        if (!isProgress)
        {
            isProgress = true;
            progress = StartCoroutine(progCoroutine());
        }
        else
        {
            StopCoroutine(progress);
            progress = StartCoroutine(progCoroutine());
        }
    }

    private void Update()
    {
    }

    IEnumerator progCoroutine()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.TIMER);
        string minutes;
        string seconds;
        while(Managers.Data.QuestProgress.remainTimer >= 0)
        {
            Managers.Data.QuestProgress.remainTimer -= Time.deltaTime; 
            minutes = Mathf.Floor(Managers.Data.QuestProgress.remainTimer / 60).ToString("00");
            seconds = (Managers.Data.QuestProgress.remainTimer % 60).ToString("00");
            timeText.text = string.Format("{0}:{1}", minutes, seconds);
            yield return null;
        }

        yield return null;
        isProgress = false;
        Managers.Data.QuestProgress.remainTimer =  -1;
        this.gameObject.SetActive(false);
        Managers.Quest.QuestFail.Invoke();
    }


}
