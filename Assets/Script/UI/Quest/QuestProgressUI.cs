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
    [SerializeField] private TextMeshProUGUI populationText;


    void Start()
    {
        
        gameObject.SetActive(false);

        Managers.Quest.SetQuestUI -= SetQuestProgress;
        Managers.Quest.SetQuestUI += SetQuestProgress;

        Managers.Quest.SetQuestUIA -= SetQuestProgressA;
        Managers.Quest.SetQuestUIA += SetQuestProgressA;

        Managers.Quest.QuestFail -= UnsetQuestProgress;
        Managers.Quest.QuestFail += UnsetQuestProgress;

        Managers.Quest.QuestSuccess -= UnsetQuestProgress;
        Managers.Quest.QuestSuccess += UnsetQuestProgress;

        Managers.Data.questUpdateDelegate = (int idx) => UpdateText(idx);
        Managers.Data.questPopulationDelegate = (int idx) => UpdatePopulation(idx);

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
        Managers.Quest.SetQuestUIA -= SetQuestProgressA;
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
            ingrImages[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManagerEx.Instance.qpDatas.questItems[i] + "/\n" + tmp[i].cnt.ToString();
            ingrImages[i].gameObject.SetActive(true);
        }
        for (int i = tmp.Count; i < ingrImages.Length; i++)
        {
            ingrImages[i].gameObject.SetActive(false);
        }

        populationText.gameObject.SetActive(false);
    }

    private void SetQuestProgressA(int id)
    {
        gameObject.SetActive(true);
        planetName.text = Managers.Resource.GetQuestData(id).QuestName;

        for (int i = 0; i < ingrImages.Length; i++)
        {
            ingrImages[i].gameObject.SetActive(false);
        }

        UpdatePopulation(id);
        populationText.gameObject.SetActive(true);
    }


    private void UnsetQuestProgress()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            GameManagerEx.Instance.qpDatas.questItems[i] = 0;
        }
    }

    private void UpdateText(int idx)
    {
        var tmp = Managers.Resource.GetQuestData(Managers.Data.QuestProgress.inProgressId).Ingredients;
        if (idx >= tmp.Count) { return; }
        ingrImages[idx].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManagerEx.Instance.qpDatas.questItems[idx] + "/\n" + tmp[idx].cnt.ToString();

        bool once = false;
        for (int i = 0; i < tmp.Count; i++)
        {
            if (GameManagerEx.Instance.qpDatas.questItems[i] < tmp[i].cnt)
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

    private void UpdatePopulation(int idx)
    {
        populationText.text = GameManagerEx.Instance.qpDatas.populations[idx].ToString() + "\n/";
    }


}
