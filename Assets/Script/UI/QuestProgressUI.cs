using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private Image[] ingrImages = new Image[3];


    void Start()
    {
        gameObject.SetActive(false);
        Managers.Quest.SetQuestUI = SetQuestProgress;
    }

    private void SetQuestProgress(int id)
    {
        gameObject.SetActive(true); 
        planetName.text = Managers.Resource.GetQuestData(id).QuestName;

        var tmp = Managers.Resource.GetQuestData(id).Ingredients;

        for (int i = 0; i < tmp.Count; i++)
        {
            ingrImages[i].sprite = Managers.Resource.GetItemSprite(tmp[i].id);
            ingrImages[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tmp[i].cnt.ToString();
            ingrImages[i].gameObject.SetActive(true);
        }
        for (int i = tmp.Count; i < ingrImages.Length; i++)
        {
            ingrImages[i].gameObject.SetActive(false);
        }
    }

    private void UnsetQuestProgress()
    {
        gameObject.SetActive(false);
    }

}
