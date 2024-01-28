using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDictInfo : MonoBehaviour
{
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoDesc;

    [SerializeField] private TextMeshProUGUI takenTimeText;

    [SerializeField] private Image outputImage;
    [SerializeField] private Image[] ingrImages = new Image[3];
    [SerializeField] private TextMeshProUGUI makingBuilding;

    [SerializeField]
    public int id = -1;

    public void SetItemDictInfo(int id)
    {
        this.id = id;
        infoImage.sprite = outputImage.sprite = Managers.Resource.GetItemSprite(id);
        var tmpData = Managers.Resource.GetItemData(id);
        infoName.text = tmpData.Name;
        infoDesc.text = tmpData.Description;

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

        makingBuilding.text = "made in : "+ Managers.Resource.GetBuildingData(tmpData.MakingBuildingId).Name;
    }

    private void OnDisable()
    {
        id = -1;
    }




}
