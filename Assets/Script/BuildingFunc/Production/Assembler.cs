using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Assembler : Production
{
    const int ID = 107;

    [SerializeField]
    private GameObject nextBelt;
    private List<Ingredient> ings;

    [SerializeField]
    private int[] stores;
    Coroutine assemblerCoroutine;

    private bool initOnce = false;

    public override void Init(int itemId)
    {
        if (!initOnce)
        {
            SetOutputItemId(itemId);
            assemblerCoroutine = StartCoroutine(AssemblerCoroutine());

            initOnce = true;
        }
    }

    private void Start()
    {
        Init(Managers.Resource.GetBuildingData(ID).OutputIds[0]);
    }


    private IEnumerator AssemblerCoroutine()
    {
        while (true)
        {
            if (nextBelt == null)
                nextBelt = Managers.Map.FindBeltFromBuilding(this, transform.position);
            else
            {
                if (CheckIngsPrepared())
                {
                    yield return new WaitForSeconds(Managers.Resource.GetItemData(outputItemId).ProductTime / Managers.Resource.GetBuildingData(ID).Speed);

                    while (nextBelt == null || !nextBelt.GetComponent<BuildingBase>().IsTransferAble(outputItemId, 0))
                    {
                        if (nextBelt == null)
                            nextBelt = Managers.Map.FindBeltFromBuilding(this, transform.position);
                        yield return new WaitForFixedUpdate();
                    }
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

    private void SetOutputItemId(int id)
    {


        this.outputItemId = id;

        ings = Managers.Resource.GetItemData(outputItemId).Ingredients;
        stores = new int[ings.Count];
        for (int i = 0; i < ings.Count; i++) { stores[i] = 0; }
    }
    public override void ChangeOutputItemId(int id)
    {
        StopCoroutine(assemblerCoroutine);
        SetOutputItemId(id);
        assemblerCoroutine = StartCoroutine(AssemblerCoroutine());
    }

    public override void EraseNextBelt(int rot)
    {
        nextBelt = null;
    }
}
