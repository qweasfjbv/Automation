using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MiningMachine : Mining
{
    const int ID = 102;

    [SerializeField]
    private int veinId;
    private Coroutine miningCoroutine;

    private void Start()
    {
        transform.GetChild(0).GetComponent<Animator>().Play(Managers.Anim.GetAnimId(ID), 0, Managers.Anim.GetAnimTime(ID));

        nextBelt = null;
        miningCoroutine = null;
        veinId = Managers.Map.GetTileOnPoint(transform.position).terrainInfo;
    }

    private void Update()
    {

        if (nextBelt == null)
        {
            GameObject tmpGo = Managers.Map.FindBeltFromBuilding(this, transform.position);
            if (tmpGo != null) nextBelt = tmpGo.GetComponent<Belt>();
            else nextBelt = null;

        }
        else
        {
            if (veinId >= 1 && veinId <= 5 && miningCoroutine == null)
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

            yield return new WaitForSeconds(Managers.Resource.GetItemData(Managers.Resource.GetVeinData(veinId).OreID).ProductTime / Managers.Resource.GetBuildingData(ID).Speed);
        }
    }


}