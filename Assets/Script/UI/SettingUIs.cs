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
    private GameObject itemDictInfo;
    [SerializeField]
    private GameObject buildingDictButton;
    [SerializeField]
    private GameObject buildingDict;
    [SerializeField]
    private GameObject buildingDictInfo;
    [SerializeField]
    private GameObject settingButton;
    [SerializeField]
    private GameObject setting;

    private Button[] itemButtonList;
    private Button[] buildingButtonList;

    private GameObject curOpeningUI = null;

    int minItemId;
    int maxItemId;

    int minBuildingId;
    int maxBuildingId;

    private void Awake()
    {


        itemDictButton.GetComponent<Button>().onClick.AddListener(() => OnItemDictButton());
        itemButtonList = itemDict.GetComponentsInChildren<Button>();

        buildingDictButton.GetComponent<Button>().onClick.AddListener(() => OnBuildingDictButton());
        buildingButtonList = buildingDict.GetComponentsInChildren<Button>();

        settingButton.GetComponent<Button>().onClick.AddListener(() => OnSettingButton());

        itemDict.SetActive(false);
        itemDictInfo.SetActive(false);

        buildingDict.SetActive(false);
        buildingDictInfo.SetActive(false);

    }

    private void Start()
    {

        minItemId = ResourceManager.ITEMOFFSET;
        maxItemId = minItemId + Managers.Resource.GetItemCount() -1;

        minBuildingId = ResourceManager.BUILDINGOFFSET;
        maxBuildingId = minBuildingId + Managers.Resource.GetBuildingCount()-1;

    }

    private void OnItemDictButton()
    {
        if (itemDict.activeSelf == true)
        {
            itemDictInfo.SetActive(false);
            itemDict.SetActive(false);
            curOpeningUI = null;
        }
        else
        {
            if (curOpeningUI != null)
            {
                curOpeningUI.SetActive(false);
                buildingDictInfo.SetActive(false);
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
                    int idx = i + minItemId;
                    itemButtonList[i].onClick.RemoveAllListeners();
                    itemButtonList[i].onClick.AddListener(() => OnItemClicked(idx));
                }
            }
        }
    }

    private void OnItemClicked(int id)
    {
        if(itemDictInfo.GetComponent<ItemDictInfo>().id == id)
        {
            itemDictInfo.SetActive(false);
        }
        else
        {
            itemDictInfo.GetComponent<ItemDictInfo>().SetItemDictInfo(id);
            itemDictInfo.SetActive(true);
        }
    }

    private void OnBuildingDictButton()
    {
        if (buildingDict.activeSelf == true)
        {
            buildingDictInfo.SetActive(false);
            buildingDict.SetActive(false);
            curOpeningUI = null;
        }
        else
        {
            if (curOpeningUI != null)
            {
                itemDictInfo.SetActive(false);
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
                    int idx = i + minBuildingId;
                    buildingButtonList[i].onClick.RemoveAllListeners();
                    buildingButtonList[i].onClick.AddListener(() => OnBuildingClicked(idx));
                }
            }
        }
    }
    
    public void OnBuildingClicked(int id)
    {
        
        if (buildingDictInfo.GetComponent<ItemDictInfo>().id == id)
        {
            buildingDictInfo.SetActive(false);
        }
        else
        {
            buildingDictInfo.GetComponent<ItemDictInfo>().SetItemDictInfo(id);
            buildingDictInfo.SetActive(true);
        }
    }

    private void OnSettingButton()
    {
        if (setting.activeSelf == false)
        {
            if (curOpeningUI != null)
            {
                buildingDictInfo.SetActive(false);
                itemDictInfo.SetActive(false);
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
                buildingDictInfo.SetActive(false);
                itemDictInfo.SetActive(false);
                curOpeningUI.SetActive(false);
                curOpeningUI = null;
            }
        }
    }


}
