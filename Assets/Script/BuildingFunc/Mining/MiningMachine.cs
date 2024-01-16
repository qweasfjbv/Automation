using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MiningMachine : BuildingBase
{
    [SerializeField] GameObject itemPrefab;

    const int ID = 102;

    
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
            GameObject tmpGo = Managers.Map.FindBeltFromBuilding(transform.position);
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
            yield return new WaitUntil(() => nextBelt != null && nextBelt.IsTransferAble(Managers.Resource.GetVeinData(veinId).OreID, 0));
            nextBelt.SetBeltId(Managers.Resource.GetVeinData(veinId).OreID, 0);

            yield return new WaitForSeconds(Managers.Resource.GetVeinData(veinId).MiningTime/Managers.Resource.GetBuildingData(ID).Speed);
        }
    }


    public override void SetBeltId(int id, int rot = 0)
    {
        return;
    }
    public override bool IsTransferAble(int id, int rot)
    {
        return false;
    }


}