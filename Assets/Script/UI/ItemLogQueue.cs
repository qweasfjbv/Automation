using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogQueue : MonoBehaviour
{
    [SerializeField]
    private GameObject itemLogPrefab;

    // Circular Queue
    private const int QSIZE = 4;
    private float itemLogOffset;

    private ItemLog[] itemLogQueue = new ItemLog[QSIZE];
    private int start = 0;
    private int end = 0;

    private void Awake()
    {
        itemLogOffset = itemLogPrefab.GetComponent<RectTransform>().rect.height;
    }

    private void Update()
    {
        if (start != end && itemLogQueue[(start + 1) % QSIZE] == null)
        {
            start = (start + 1) % QSIZE;
        }
    }

    public void AddItemLog(int id, int cnt)
    {
        end = (end + 1) % QSIZE;
        if (end == start && itemLogQueue[(start + 1) % QSIZE] != null)
        {
            start = (start + 1) % QSIZE;
            Destroy(itemLogQueue[start].gameObject);
        }

        itemLogQueue[end] = Instantiate(itemLogPrefab, transform).GetComponent<ItemLog>();
        itemLogQueue[end].SetItemLog(id, cnt);

        ElevateAllLog();
    }

    // 1: 재료부족
    // 2: 만들수없는 아이템
    public void AddErrLog(int id, int num)
    {
        string s = "";
        switch (num)
        {
            case 1:
                s = "insufficient ingredients!";
                break;
            case 2:
                s = "The item cannot be crafted.";
                break;
            default:
                return;
        }
        end = (end + 1) % QSIZE;
        if (end == start && itemLogQueue[(start + 1) % QSIZE] != null)
        {
            start = (start + 1) % QSIZE;
            Destroy(itemLogQueue[start].gameObject);
        }

        itemLogQueue[end] = Instantiate(itemLogPrefab, transform).GetComponent<ItemLog>();
        itemLogQueue[end].SetErrLog(id, s);


        ElevateAllLog();

    }

    private void ElevateAllLog()
    {
        // [start+1, end)
        int cur = (start + 1) % QSIZE;

        while (cur != end)
        {
            if (itemLogQueue[cur] != null)
            {
                itemLogQueue[cur].transform.position += itemLogQueue[cur].transform.parent.TransformVector(new Vector3(0, itemLogOffset, 0));
            }

            cur = (cur + 1) % QSIZE;
        }
    }
}
