using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private bool isActive;
    [SerializeField] private Slider craftSlider;
    [SerializeField] private GameObject craftingQueue;
    [SerializeField] private GameObject invenItem;
    [SerializeField] private GameObject invenBuilding;


    [SerializeField]
    List<int> invenItemList;
    List<int> invenBuildingList;

    private Button[] craftButton;
    private int[] craftButtonId;
    private Coroutine craftCoroutine;
    private bool isCoroutineRunning = false;

    private Button[] itemButtons;
    private Button[] buildingButtons;

    const int minItemId = 11;
    const int maxItemId = 35;

    const int minBuildingId = 101;
    const int maxBuildingId = 111;

    private int rear = 0;
    private int end = 0;


    private void Awake()
    {
        isActive = gameObject.activeSelf;
        itemButtons = invenItem.GetComponentsInChildren<Button>();
        buildingButtons = invenBuilding.GetComponentsInChildren<Button>();
        craftButton = craftingQueue.GetComponentsInChildren<Button>();
        craftSlider.gameObject.SetActive(false);
        for (int i = 0; i < craftButton.Length; i++)
        {
            craftButton[i].transform.parent.gameObject.SetActive(false);
        }

        craftButtonId = new int[craftButton.Length];

        if (!isActive)
        {
            gameObject.SetActive(true);
        }
        this.transform.position = new Vector3(0, 1000, 0);
    }
    private void Start()
    {
        invenItemList = Managers.Data.LoadInvenItem();
        invenBuildingList = Managers.Data.LoadInvenBuilding();
    }

    private void Update()
    {
        if (!isCoroutineRunning && craftButton[rear].transform.parent.gameObject.activeSelf)
        {
            isCoroutineRunning = true;
            craftCoroutine = StartCoroutine(ItemCraftCoroutine(craftButtonId[rear], rear));
            rear = (rear + 1) % craftButtonId.Length;
        }
    }

    public void GetItem(int id, int cnt)
    {

        // Log 띄우기

        if (id >= 100)
        {
            invenBuildingList[id - Managers.Resource.BUILDINGOFFSET] += cnt;
        }
        else
        {
            invenItemList[id - Managers.Resource.ITEMOFFSET] += cnt;
        }
    }
    private bool MakeItem(int id)
    {
        var tmpData = Managers.Resource.GetItemData(id);

        bool isPossible = true;
        for (int i = 0; i < tmpData.Ingredients.Count; i++)
        {
            if (!CheckEnoughItem(tmpData.Ingredients[i].id, tmpData.Ingredients[i].cnt))
            {
                isPossible = false; break;
            }
        }

        if (isPossible)
        {
            for (int i = 0; i < tmpData.Ingredients.Count; i++)
            {
                UseItem(tmpData.Ingredients[i].id, tmpData.Ingredients[i].cnt);
            }
            return true;
        }

        return false;
    }

    private bool CheckEnoughItem(int id, int cnt)
    {
        return invenItemList[id - Managers.Resource.ITEMOFFSET] >= cnt;
    }
    private void UseItem(int id, int cnt)
    {
        if(id >= 100)
        {
            invenItemList[id - Managers.Resource.BUILDINGOFFSET] -= cnt;
        }
        else
        {

            invenItemList[id - Managers.Resource.ITEMOFFSET] -= cnt;
        }
    }
    public void Toggle()
    {
        if (isActive)
        {
            // 끌때

            this.transform.localPosition = new Vector3(0, 1000, 0);
        }
        else
        {
            UpdateInven();
            this.transform.localPosition = new Vector3(0, 0, 0);
        }

        isActive = !isActive;
    }
    private void UpdateInven()
    {


        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i + minItemId > maxItemId)
            {
                itemButtons[i].transform.GetComponent<Image>().sprite = null;
                itemButtons[i].transform.GetComponent<Image>().color = Color.clear;
                itemButtons[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                itemButtons[i].transform.GetComponent<Image>().sprite = Managers.Resource.GetItemSprite(i + minItemId);
                itemButtons[i].transform.GetComponent<Image>().color = Color.white;
                itemButtons[i].transform.parent.gameObject.SetActive(true);
                int idx = i + minItemId;
                itemButtons[i].onClick.RemoveAllListeners();
                itemButtons[i].onClick.AddListener(() => OnItemClicked(idx));
            }
        }

        for (int i = 0; i < buildingButtons.Length; i++)
        {
            if (i + minBuildingId > maxBuildingId)
            {
                buildingButtons[i].transform.GetComponent<Image>().sprite = null;
                buildingButtons[i].transform.GetComponent<Image>().color = Color.clear;
                buildingButtons[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                buildingButtons[i].transform.GetComponent<Image>().sprite = Managers.Resource.GetBuildingSprite(i + minBuildingId);
                buildingButtons[i].transform.GetComponent<Image>().color = Color.white;
                buildingButtons[i].transform.parent.gameObject.SetActive(true);
                int idx = i + minBuildingId;
                buildingButtons[i].onClick.RemoveAllListeners();
                buildingButtons[i].onClick.AddListener(() => OnItemClicked(idx));
            }
        }
    }

    private void OnItemClicked(int id)
    {
        if ((end + 1) % craftButton.Length == rear)
        {
            Debug.Log("Queue FULL!");
            return;
        }

        if (MakeItem(id))
        {
            AddCircularQueue(id);
        }
        else
        {
            Debug.Log("크래프팅 아이템 부족!!");
        }
    }

    private void AddCircularQueue(int id)
    {
        craftButton[end].GetComponent<Image>().sprite = Managers.Resource.GetItemSprite(id);
        craftButtonId[end] = id;
        craftButton[end].transform.parent.SetAsLastSibling();
        craftButton[end].transform.parent.gameObject.SetActive(true);
        craftButton[end].onClick.RemoveAllListeners();
        int e = end;
        craftButton[end].onClick.AddListener(() => OnCraftingButtonClicked(e));
        end = (end + 1) % craftButton.Length;
    }

    public void OnCraftingButtonClicked(int idx)
    {

        int id = craftButtonId[idx];

        if ((idx+1) % craftButtonId.Length == rear)
        {
            StopCoroutine(craftCoroutine);
            craftSlider.gameObject.SetActive(false);
            isCoroutineRunning = false;
        }
        for (int i = 0; i < Managers.Resource.GetItemData(id).Ingredients.Count; i++)
        {
            GetItem(Managers.Resource.GetItemData(id).Ingredients[i].id, Managers.Resource.GetItemData(id).Ingredients[i].cnt);
        }
        craftButton[idx].transform.parent.gameObject.SetActive(false);

    }


    private IEnumerator ItemCraftCoroutine(int id, int idx)
    {
        Debug.Log("Rear and END : " + rear + ", " + end);
        craftSlider.value = 0;
        craftSlider.gameObject.SetActive(true);
        float elapsedTime = 0f;
        float craftTime = Managers.Resource.GetItemData(id).ProductTime;

        while (elapsedTime < craftTime)
        {
            craftSlider.value = elapsedTime / craftTime;
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        GetItem(id, 1);
        yield return null;
        craftButton[idx].transform.parent.gameObject.SetActive(false);
        craftButtonId[idx] = -1;
        isCoroutineRunning = false;
        craftSlider.gameObject.SetActive(false);
    }

}
