using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PoolManager
{

    Stack<GameObject> beltPool = new Stack<GameObject>();

    private Transform _root;
    public void Init(int cnt)
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }

        SetPooling(cnt);
    }

    private void SetPooling(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var tmp = GameObject.Instantiate(Managers.Resource.GetBuildingData(101).Prefab);
            tmp.SetActive(false);
            tmp.transform.parent = _root;
            beltPool.Push(tmp);
        }

    }

    public GameObject Pop(Vector3 pos, Vector3 rot)
    {
        if (beltPool.Count < 0)
        {
            Debug.Log("Pool Dried");
            return null;
        }

        GameObject tmp = beltPool.Pop();
        tmp.transform.parent = null;
        tmp.transform.position = pos;
        tmp.transform.rotation = Quaternion.Euler(rot);


        tmp.SetActive(true);
        return tmp;
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = _root;
        beltPool.Push(obj);
    }

    public void Clear()
    {
        while(beltPool.Count > 0)
        {
            var tmp = beltPool.Pop();
            GameObject.Destroy(tmp);
        }

        beltPool.Clear();
    }
}