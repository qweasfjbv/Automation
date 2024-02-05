using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private Image[] ingrImages = new Image[3];
    [SerializeField] private GameObject progressTime;
    [SerializeField] private int[] itemCount;

    void Start()
    {
        itemCount = Managers.Data.LoadQuestProgress();

        gameObject.SetActive(false);
        progressTime.SetActive(false);
        Managers.Quest.SetQuestUI -= SetQuestProgress;
        Managers.Quest.SetQuestUI += SetQuestProgress;

        Managers.Quest.QuestFail -= UnsetQuestProgress;
        Managers.Quest.QuestFail += UnsetQuestProgress;

        Managers.Quest.QuestSuccess -= UnsetQuestProgress;
        Managers.Quest.QuestSuccess += UnsetQuestProgress;

        Managers.Data.questUpdateDelegate = (int idx) => UpdateText(idx);

        if (Managers.Data.QuestProgress.inProgressId != -1)
        {
            Managers.Quest.SetQuestUI.Invoke(Managers.Data.QuestProgress.inProgressId);
        }
    }

    private void OnDestroy()
    {
        Managers.Quest.SetQuestUI -= SetQuestProgress;
        Managers.Quest.QuestFail -= UnsetQuestProgress;
        Managers.Quest.QuestSuccess -= UnsetQuestProgress;
        Managers.Data.questUpdateDelegate = null;
    }

    private void SetQuestProgress(int id)
    {
        gameObject.SetActive(true); 
        planetName.text = Managers.Resource.GetQuestData(id).QuestName;

        var tmp = Managers.Resource.GetQuestData(id).Ingredients;

        for (int i = 0; i < tmp.Count; i++)
        {
            ingrImages[i].sprite = Managers.Resource.GetItemSprite(tmp[i].id);
            ingrImages[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemCount[i] + "/\n" + tmp[i].cnt.ToString();
            ingrImages[i].gameObject.SetActive(true);
        }
        for (int i = tmp.Count; i < ingrImages.Length; i++)
        {
            ingrImages[i].gameObject.SetActive(false);
        }

        progressTime.SetActive(true);
        if (Managers.Data.QuestProgress.remainTimer < 0)
        {
            progressTime.GetComponent<ProgressTime>().SetProgTime(Managers.Resource.GetQuestData(id).TimeLimit);
        }
        else
        {
            progressTime.GetComponent<ProgressTime>().SetProgTime(Managers.Data.QuestProgress.remainTimer);
        }
    }
    private void UnsetQuestProgress()
    {
        gameObject.SetActive(false);
        progressTime.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            itemCount[i] = 0;
        }
    }

    private void UpdateText(int idx)
    {
        var tmp = Managers.Resource.GetQuestData(Managers.Data.QuestProgress.inProgressId).Ingredients;
        if (idx >= tmp.Count) { return; }
        ingrImages[idx].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemCount[idx] + "/\n" + tmp[idx].cnt.ToString();

        bool once = false;
        for (int i = 0; i < tmp.Count; i++)
        {
            if (itemCount[i] < tmp[i].cnt)
            {
                once = true;
                break;
            }
        }

        if (!once)
        {
            Managers.Quest.QuestSuccess.Invoke();
        }
    }



}
