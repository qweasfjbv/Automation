using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIs : MonoBehaviour
{
    [SerializeField]
    private GameObject itemDictButton;
    [SerializeField]
    private GameObject itemDict;
    [SerializeField]
    private GameObject buildingDictButton;
    [SerializeField]
    private GameObject buildingDict;
    [SerializeField]
    private GameObject settingButton;
    [SerializeField]
    private GameObject setting;

    private Button[] itemButtonList;
    private Button[] buildingButtonList;

    private GameObject curOpeningUI = null;

    const int minItemId = 11;
    const int maxItemId = 35;

    const int minBuildingId = 101;
    const int maxBuildingId = 111;

    private void Awake()
    {

        itemDictButton.GetComponent<Button>().onClick.AddListener(() => OnItemDictButton());
        itemButtonList = itemDict.GetComponentsInChildren<Button>();

        buildingDictButton.GetComponent<Button>().onClick.AddListener(() => OnBuildingDictButton());
        buildingButtonList = buildingDict.GetComponentsInChildren<Button>();

        settingButton.GetComponent<Button>().onClick.AddListener(() => OnSettingButton());
    }

    private void OnItemDictButton()
    {
        if (itemDict.activeSelf == true)
        {
            itemDict.SetActive(false);
            curOpeningUI = null;
        }
        else
        {
            if (curOpeningUI != null)
            {
                curOpeningUI.SetActive(false);
            }
            itemDict.SetActive(true);
            curOpeningUI = itemDict;

            for (int i = 0; i < itemButtonList.Length; i++)
            {
                if (i + minItemId > maxItemId)
                {
                    itemButtonList[i].transform.GetComponent<Image>().sprite = null;
                    itemButtonList[i].transform.GetComponent<Image>().color = Color.clear;
                    itemButtonList[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    itemButtonList[i].transform.GetComponent<Image>().sprite = Managers.Resource.GetItemSprite(i + minItemId);
                    itemButtonList[i].transform.GetComponent<Image>().color = Color.white;
                    itemButtonList[i].transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnBuildingDictButton()
    {
        if (buildingDict.activeSelf == true)
        {
            buildingDict.SetActive(false);
            curOpeningUI = null;
        }
        else
        {
            if (curOpeningUI != null)
            {
                curOpeningUI.SetActive(false);
            }
            buildingDict.SetActive(true);
            curOpeningUI = buildingDict;

            for (int i = 0; i < buildingButtonList.Length; i++)
            {
                if (i + minBuildingId > maxBuildingId)
                {
                    buildingButtonList[i].transform.GetComponent<Image>().sprite = null;
                    buildingButtonList[i].transform.GetComponent<Image>().color = Color.clear;
                    buildingButtonList[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    buildingButtonList[i].transform.GetComponent<Image>().sprite = Managers.Resource.GetBuildingSprite(i + minBuildingId);
                    buildingButtonList[i].transform.GetComponent<Image>().color = Color.white;
                    buildingButtonList[i].transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }
    
    private void OnSettingButton()
    {
        if (setting.activeSelf == false)
        {
            if (curOpeningUI != null)
            {
                curOpeningUI.SetActive(false);
            }
            setting.SetActive(true);
            curOpeningUI = setting;
        }
        else
        {
            setting.SetActive(false);
            curOpeningUI = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnItemDictButton();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            OnBuildingDictButton();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (curOpeningUI == null)
            {
                OnSettingButton();
            }
            else
            {
                curOpeningUI.SetActive(false);
                curOpeningUI = null;
            }
        }
    }
}
