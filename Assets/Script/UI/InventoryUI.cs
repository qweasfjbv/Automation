using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField]
    private int[] craftButtonId;
    [SerializeField]
    private int[] craftWaitItemIds;
    private Coroutine craftCoroutine;
    private bool isCoroutineRunning = false;

    private Button[] itemButtons;
    private Button[] buildingButtons;

    const int minItemId = 11;
    const int maxItemId = 35;

    const int minBuildingId = 101;
    const int maxBuildingId = 111;

    private int front = 0, rear = 0;

    private bool Dequeue()
    {
        if (rear == front) return false;

        front = (front + 1) % craftButtonId.Length;
        craftButtonId[front] = -1;

        return true;
    }

    private bool Enqueue(int id)
    {
        if ((rear + 1) % craftButtonId.Length == front)
        {
            return false;
        }
        rear = (rear + 1) % craftButtonId.Length;
        craftButtonId[rear] = id;

        return true;
    }
    private void EraseQueue(int idx)
    {
        //idx 는 craftButton의 idx;
        int start = -1;
        for (int i = 0; i < craftButtonId.Length; i++)
        {
            if (craftButtonId[i] == idx)
            {
                start = i; break;
            }
        }
        if (start == -1) return;

        while (start != rear)
        {
            craftButtonId[start] = craftButtonId[(start + 1) % craftButtonId.Length];
            start = (start + 1) % craftButtonId.Length;
        }

        if (start == rear)
        {
            craftButtonId[start] = craftButtonId[(start + 1) % craftButtonId.Length];
        }
        rear--;
        if (rear < 0) rear += craftButtonId.Length;
    }

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

        craftButtonId = new int[craftButton.Length+1];
        for (int i = 0; i <= craftButton.Length; i++)
        {
            craftButtonId[i] = -1;
        }
        craftWaitItemIds = new int[craftButton.Length];

        for (int i = 0; i < craftWaitItemIds.Length; i++)
        {
            craftWaitItemIds[i] = -1;
        }

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
        if (!isCoroutineRunning && craftButtonId[(front+1)% craftButtonId.Length] != -1)
        {
            isCoroutineRunning = true;
            craftCoroutine = StartCoroutine(ItemCraftCoroutine(craftWaitItemIds[craftButtonId[(front + 1) % craftButtonId.Length]], craftButtonId[(front + 1) % craftButtonId.Length]));

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

        if (MakeItem(id))
        {
            if (!AddQueue(id)) Debug.Log("Queue Full");

        }
        else
        {
            Debug.Log("크래프팅 아이템 부족!!");
        }
    }

    private bool AddQueue(int id)
    {
        int tmpIdx = -1;
        for (int i = 0; i < craftButton.Length; i++)
        {
            if (!craftButton[i].transform.parent.gameObject.activeSelf) {
                tmpIdx = i;
                break;
                }
        }
        if (tmpIdx == -1)
        {
            return false;
        }
        else
        {

            craftButton[tmpIdx].GetComponent<Image>().sprite = Managers.Resource.GetItemSprite(id);
            Enqueue(tmpIdx);
            craftWaitItemIds[tmpIdx] = id;
            craftButton[tmpIdx].transform.parent.SetAsLastSibling();
            craftButton[tmpIdx].transform.parent.gameObject.SetActive(true);
            craftButton[tmpIdx].onClick.RemoveAllListeners();
            int i = id; int e = tmpIdx;
            craftButton[tmpIdx].onClick.AddListener(() => OnCraftingButtonClicked(i, e));

            return true;
        }
    }

    public void OnCraftingButtonClicked(int id, int idx)
    {

        if (craftButtonId[(front + 1) % craftButtonId.Length] == idx)
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

        EraseQueue(idx);
    }
    
    
    private IEnumerator ItemCraftCoroutine(int id, int idx)
    {
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
        Dequeue();
        isCoroutineRunning = false;
        craftSlider.gameObject.SetActive(false);
    }

}
