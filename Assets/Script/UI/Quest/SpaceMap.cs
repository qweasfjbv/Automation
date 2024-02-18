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
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI description;

    private int childOffset = 1;

    private Vector2 sizeWithDes = new Vector2(300, 300);
    private Vector2 sizeWithoutDes = new Vector2(300, 180);

    //planetInfo : Description있으면 Height 300
    //      없으면 170

    private void Start()
    {
        planetInfo.SetActive(false);
    }

    private void OnDisable()
    {
        SoundManager.Instance.ChangeBGM(Define.BgmType.GAME);
    }

    private void OnEnable()
    {
        SoundManager.Instance.ChangeBGM(Define.BgmType.SPACE);
    }

    public void Toggle()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void PointerEnter(int id)
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);

        if (id > Managers.Data.QuestProgress.successId)
        {
            transform.GetChild(id + childOffset).GetChild(0).gameObject.SetActive(true);
            SetOutputSetting(id);
        }
        else
        {
            transform.GetChild(id + childOffset).GetChild(0).gameObject.SetActive(true);
            SetSuccessQuest(id);
        }
    }

    public void PointerExit(int id)
    {
        transform.GetChild(id + childOffset).GetChild(0).gameObject.SetActive(false);
        planetInfo.SetActive(false);
    }


    public void OnClick(int id)
    {
        if (Managers.Resource.GetQuestData(id).Ingredients.Count == 0) return;

        if (id > Managers.Data.QuestProgress.successId)
        {
            Managers.Quest.SetQuestId(id);
            transform.GetChild(id + childOffset).GetChild(0).gameObject.SetActive(false);
            planetInfo.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {

            Managers.Quest.SetQuestIdAfterClear(id);
            transform.GetChild(id + childOffset).GetChild(0).gameObject.SetActive(false);
            planetInfo.SetActive(false);
            this.gameObject.SetActive(false);
        }
        SoundManager.Instance.PlaySfxSound(Define.SoundType.GETQUEST);
    }

    private void SetOutputSetting(int id)
    {

        planetInfo.SetActive(true);
        description.gameObject.SetActive(true);
        RectTransform child = transform.GetChild(id + childOffset).GetComponent<RectTransform>();
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

        if (tmp.Count == 0)
        {
            description.gameObject.SetActive(false);
            planetInfo.GetComponent<RectTransform>().sizeDelta = sizeWithoutDes;
            populationText.gameObject.SetActive(true);
            populationText.text = "IMPOSSIBLE";
            return;
        }
        else
        {

            planetInfo.GetComponent<RectTransform>().sizeDelta = sizeWithDes;
        }


        description.text = Managers.Resource.GetQuestData(id).QuestDescription;
        populationText.gameObject.SetActive(false);

    }

    private void SetSuccessQuest(int id)
    {
        description.gameObject.SetActive(true);
        planetInfo.SetActive(true);
        planetInfo.GetComponent<RectTransform>().sizeDelta = sizeWithDes;

        RectTransform child = transform.GetChild(id + childOffset).GetComponent<RectTransform>();
        planetInfo.transform.localPosition = new Vector3(child.localPosition.x + child.rect.width, child.localPosition.y, 0);
        planetName.text = Managers.Resource.GetQuestData(id).QuestName + " (habitable)";

        for (int i = 0; i < ingrImages.Length; i++)
        {
            ingrImages[i].gameObject.SetActive(false);
        }


        populationText.text = GameManagerEx.Instance.qpDatas.populations[id].ToString() + "\n/ " + Managers.Resource.GetQuestData(id).PopulationLimit.ToString("N0");


        description.text = Managers.Resource.GetQuestData(id).QuestDescription;
        populationText.gameObject.SetActive(true);
    }


}
