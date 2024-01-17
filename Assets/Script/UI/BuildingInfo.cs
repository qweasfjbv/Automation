using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoDesc;
    [SerializeField] private GameObject outputSetting;
    [SerializeField] private GameObject outputItemList;

    [SerializeField] private TextMeshProUGUI takenTimeText;

    [SerializeField] private Image outputImage;
    [SerializeField] private Image[] ingrImages = new Image[3];


    private List<Transform> outputItemSlots = new List<Transform>();

    private int buildingId = -1;
    private Production production;
    private int[] ids;

    public void Start()
    {

    }

    public void SetBuildingInfo(int id, GameObject gameObject)
    {
        outputItemList.SetActive(false);
        buildingId = id;
        production = gameObject.GetComponent<Production>();

        if(Managers.Resource.GetBuildingData(id).OutputIds.Count == 0)
        {
            outputSetting.SetActive(false);
            GetComponent<RectTransform>().sizeDelta = new Vector2(320, 100);
        }
        else
        {
            foreach (Transform child in outputItemList.transform)
            {
                outputItemSlots.Add(child.GetChild(0));
            }
            for (int i = 0; i < outputItemSlots.Count; i++)
            {
                outputItemSlots[i].gameObject.SetActive(false);
            }

            SetOutputSetting(production.OutputItemId);
            outputSetting.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = new Vector2(320, 200);
        }

        infoImage.sprite = Managers.Resource.GetBuildingSprite(id);
        infoName.text = Managers.Resource.GetBuildingData(id).Name;
        infoDesc.text = "Info Description Needed.";

    }

    public void OnSettingClicked()
    {
        var ids = Managers.Resource.GetBuildingData(buildingId).OutputIds;
        for (int i = 0; i < ids.Count; i++) 
        {
            outputItemSlots[i].GetComponent<Image>().sprite = Managers.Resource.GetItemData(ids[i]).Image;
            int idx = i;
            outputItemSlots[idx].GetComponent<Button>().onClick.RemoveAllListeners();
            outputItemSlots[idx].GetComponent<Button>().onClick.AddListener(() => ChangeOutputItem(ids[idx]));
            outputItemSlots[i].gameObject.SetActive(true);
        }

        outputItemList.SetActive(true);
    }

    private void SetOutputSetting(int id)
    {
        outputImage.sprite = Managers.Resource.GetItemSprite(id);
        takenTimeText.text = Managers.Resource.GetItemData(id).ProductTime.ToString() + " s";

        var tmp = Managers.Resource.GetItemData(id).Ingredients;
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

    private void ChangeOutputItem(int id)
    {
        production.ChangeOutputItemId(id);
        SetOutputSetting(id);
    }

}
