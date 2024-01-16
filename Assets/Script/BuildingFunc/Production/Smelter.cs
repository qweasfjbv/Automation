using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Smelter : BuildingBase
{
    const int ID = 106;
    [SerializeField]
    private int outputItemId = 21;

    private GameObject nextBelt;
    private List<Ingredient> ings;
    private int[] stores;
    
    private void Init()
    {
        ings = Managers.Resource.GetItemData(outputItemId).Ingredients;
        stores = new int[ings.Count];
        for(int i=0; i<ings.Count; i++) { stores[i] = 0; }

        StartCoroutine(SmelterCoroutine());
    }

    private void Start()
    {
        Init();
    }


    private IEnumerator SmelterCoroutine()
    {
        while (true)
        {
            if (nextBelt == null)
                nextBelt = Managers.Map.FindBeltFromBuilding(transform.position);
            else
            {
                if (CheckIngsPrepared())
                {
                    yield return new WaitForSeconds(Managers.Resource.GetItemData(outputItemId).ProductTime / Managers.Resource.GetBuildingData(ID).Speed);
                    yield return new WaitUntil(() => nextBelt != null && nextBelt.GetComponent<BuildingBase>().IsTransferAble(outputItemId, 0));
                    nextBelt.GetComponent<BuildingBase>().SetBeltId(outputItemId);
                    for (int i = 0; i < ings.Count; i++) { stores[i] = 0; }
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private bool CheckIngsPrepared()
    {
        for (int i = 0; i < ings.Count; i++)
        {
            if (stores[i] != ings[i].cnt) return false;
        }
        return true;

    }

    public override bool IsTransferAble(int id, int rot)
    {
        for (int i = 0; i < ings.Count; i++)
        {
            if (id == ings[i].id && ings[i].cnt > stores[i]) return true;
        }


        return false;
    }

    public override void SetBeltId(int id, int rot = 0)
    {
        for (int i = 0; i < ings.Count; i++)
        {
            if (id == ings[i].id && ings[i].cnt > stores[i])
            {
                stores[i]++;
            }
        }
    }
}
