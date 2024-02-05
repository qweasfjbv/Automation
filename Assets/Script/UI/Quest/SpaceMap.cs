using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpaceMap : MonoBehaviour
{
    [SerializeField]
    private GameObject planetInfo;
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private Image[] ingrImages = new Image[3];



    private void Start()
    {
        planetInfo.SetActive(false);
    }

    public void PointerEnter(int id)
    {
        if (id > Managers.Data.QuestProgress.successId)
        {
            transform.GetChild(id).GetChild(0).gameObject.SetActive(true);
            SetOutputSetting(id);
        }
        else
        {
            transform.GetChild(id).GetChild(0).gameObject.SetActive(true);
            SetSuccessQuest(id);
        }
    }
    public void PointerExit(int id)
    {
        transform.GetChild(id).GetChild(0).gameObject.SetActive(false);
        planetInfo.SetActive(false);
    }
    public void OnClick(int id)
    {
        if (id > Managers.Data.QuestProgress.successId)
        {
            Managers.Quest.SetQuestId(id);
            transform.GetChild(id).GetChild(0).gameObject.SetActive(false);
            planetInfo.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    private void SetOutputSetting(int id)
    {

        planetInfo.SetActive(true);
        RectTransform child = transform.GetChild(id).GetComponent<RectTransform>();
        planetInfo.transform.localPosition = new Vector3(child.localPosition.x + child.rect.width, child.localPosition.y, 0);
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

    private void SetSuccessQuest(int id)
    {
        planetInfo.SetActive(true);
        RectTransform child = transform.GetChild(id).GetComponent<RectTransform>();
        planetInfo.transform.localPosition = new Vector3(child.localPosition.x + child.rect.width, child.localPosition.y, 0);
        planetName.text = "SUCCESS!";

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


}
