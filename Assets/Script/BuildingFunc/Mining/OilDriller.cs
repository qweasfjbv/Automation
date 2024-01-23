using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDriller : Mining
{

    const int ID = 108;

    [SerializeField]
    private int veinId;
    private Coroutine miningCoroutine;

    private void Start()
    {
        this.GetComponentInChildren<Animator>().Play(Managers.Anim.GetAnimId(ID), 0, Managers.Anim.GetAnimTime(ID));

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
            if (veinId == 6 && miningCoroutine == null)
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
