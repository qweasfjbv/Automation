using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MiningMachine : BuildingBase
{
    [SerializeField] GameObject itemPrefab;


    
    private Belt nextBelt;
    [SerializeField]
    private int veinId;
    private Coroutine miningCoroutine;

    private void Start()
    {
        nextBelt = null;
        miningCoroutine = null;
        veinId = Managers.Map.UsingArea[(int)Mathf.Abs(Mathf.Ceil(transform.position.y)), (int)Mathf.Floor(transform.position.x)].veinId;
    }

    private void Update()
    {

        if (nextBelt == null)
        {
            GameObject tmpGo = Managers.Map.FindBeltFromBuilding(new Vector2(Mathf.Floor(transform.position.x), Mathf.Ceil(transform.position.y)));
            if (tmpGo != null) nextBelt = tmpGo.GetComponent<Belt>();
            else nextBelt = null;

        }
        else
        {
            if (veinId != -1 && miningCoroutine == null)
            {
                miningCoroutine = StartCoroutine(MiningCoroutine());
            }
        }

    }

    private IEnumerator MiningCoroutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => nextBelt.BeltItemId != -1);
            Debug.Log("check beltid : " + nextBelt.BeltItemId + ", ");
            nextBelt.BeltItemId = Managers.Resource.GetVeinData(veinId).OreID;

            yield return new WaitForSeconds(Managers.Resource.GetVeinData(veinId).MiningTime);
        }
    }

}