using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressTime : MonoBehaviour
{
    private Coroutine progress;
    private bool isProgress = false;
    private TextMeshProUGUI timeText;

    public void SetProgTime(float t)
    {
        timeText = GetComponent<TextMeshProUGUI>();
        if (!isProgress)
        {
            isProgress = true;
            progress = StartCoroutine(progCoroutine(t));
        }
        else
        {
            StopCoroutine(progress);
            progress = StartCoroutine(progCoroutine(t));
        }
    }

    IEnumerator progCoroutine(float t)
    {
        float tmpTime = t;
        string minutes;
        string seconds;
        while(tmpTime >= 0)
        {
            tmpTime -= Time.deltaTime; 
            minutes = Mathf.Floor(tmpTime / 60).ToString("00");
            seconds = (tmpTime % 60).ToString("00");
            timeText.text = string.Format("{0}:{1}", minutes, seconds);
            yield return null;
        }

        yield return null;
        isProgress = false;
        this.gameObject.SetActive(false);
        Managers.Quest.QuestFail.Invoke();
    }


}
