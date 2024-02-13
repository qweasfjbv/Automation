using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private ItemLogQueue logQueue;

    [SerializeField]
    int[] invenItemList;
    int[] invenBuildingList;

    private Button[] craftButton;
    [SerializeField]
    private int[] craftButtonId;
    [SerializeField]
    private int[] craftWaitItemIds;
    private Coroutine craftCoroutine;
    private bool isCoroutineRunning = false;

    private Button[] itemButtons;
    private Button[] buildingButtons;

    int minItemId;
    int maxItemId;

    int minBuildingId;
    int maxBuildingId;

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
        isActive = false;
        this.transform.localPosition = new Vector3(0, 1000, 0);
    }

    private void Start()
    {
        Managers.Data.itemUpdateDelegate = (int id) => UpdateItemCount(id);

        minItemId = ResourceManager.ITEMOFFSET;
        maxItemId = minItemId + Managers.Resource.GetItemCount()-1;
        minBuildingId = ResourceManager.BUILDINGOFFSET;
        maxBuildingId = minBuildingId + Managers.Resource.GetBuildingCount()-1;

        invenItemList = Managers.Data.LoadInvenItem();
        invenBuildingList = Managers.Data.LoadInvenBuilding();

        for (int i = 0; i < invenItemList.Length; i++)
        {
            UpdateItemCount(i + minItemId);
        }

        for (int i = 0; i < invenBuildingList.Length; i++)
        {
            UpdateItemCount(i + minBuildingId);
        }

        Managers.Map.BuildFunc = UseItem;
        Managers.Map.UnbuildFunc = OnGetItem;
    }

    private void Update()
    {
        if (!isCoroutineRunning && craftButtonId[(front+1)% craftButtonId.Length] != -1)
        {
            isCoroutineRunning = true;
            craftCoroutine = StartCoroutine(ItemCraftCoroutine(craftWaitItemIds[craftButtonId[(front + 1) % craftButtonId.Length]], craftButtonId[(front + 1) % craftButtonId.Length]));

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Toggle();
        }
    }

    public void OnGetItem(int id, int cnt, bool log=true)
    {
        if (log)
            logQueue.AddItemLog(id, cnt);
        if (id >= 100)
        {
            invenBuildingList[id - ResourceManager.BUILDINGOFFSET] += cnt;
        }
        else
        {
            invenItemList[id - ResourceManager.ITEMOFFSET] += cnt;
        }
        UpdateItemCount(id);
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

        if (tmpData.Ingredients.Count == 0) {
            // 만들수 없는 물건
            logQueue.AddErrLog(id, 2);
            return false; }

        if (isPossible)
        {
            for (int i = 0; i < tmpData.Ingredients.Count; i++)
            {
                UseItem(tmpData.Ingredients[i].id, tmpData.Ingredients[i].cnt);
            }
            return true;
        }

        //재료부족
        logQueue.AddErrLog(id, 1);
        return false;
    }

    private bool CheckEnoughItem(int id, int cnt)
    {
        return invenItemList[id - ResourceManager.ITEMOFFSET] >= cnt;
    }
    private bool UseItem(int id, int cnt)
    {
        if(id >= 100)
        {
            if (invenBuildingList[id - ResourceManager.BUILDINGOFFSET] <= 0) return false;
            invenBuildingList[id - ResourceManager.BUILDINGOFFSET] -= cnt;
        }
        else
        {
            if (invenItemList[id - ResourceManager.ITEMOFFSET] <= 0) return false;
            invenItemList[id - ResourceManager.ITEMOFFSET] -= cnt;
        }
        UpdateItemCount(id);

        return true;
    }
    public void Toggle()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
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
            if (!AddQueue(id))
            {
                // 큐가 가득참
            }

        }
        // 재료부족 or 재료가 없음
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
            OnGetItem(Managers.Resource.GetItemData(id).Ingredients[i].id, Managers.Resource.GetItemData(id).Ingredients[i].cnt, false);
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

        OnGetItem(id, 1);
        yield return null;
        craftButton[idx].transform.parent.gameObject.SetActive(false);
        Dequeue();
        isCoroutineRunning = false;
        craftSlider.gameObject.SetActive(false);
    }

    private void UpdateItemCount(int id)
    {
        if (id < 100)
        {
            itemButtons[id - minItemId].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = invenItemList[id - minItemId].ToString();
        }
        else
        {
            buildingButtons[id - minBuildingId].transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = invenBuildingList[id - minBuildingId].ToString();
        }
    }
}
