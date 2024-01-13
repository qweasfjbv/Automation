using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Merger : BuildingBase
{
    private GameObject nextBelt;

    private int[] beltItemIds;
    private int beltDir;


    private void Init()
    {
        nextBelt = null;
        beltItemIds = new int[4] { -1, -1, -1, -1 };
        beltDir = 0;
    }

    private void Start()
    {
        Init();
        StartCoroutine(MergeCoroutine());
    }

    public override void SetBeltId(int id, int rot)
    {
        beltItemIds[rot] = id;
    }

    public override bool IsTransferAble(int rot)
    {
        return beltItemIds[rot] == -1;
    }


    private IEnumerator MergeCoroutine()
    {
        while (true)
        {
            if (nextBelt == null)
                nextBelt = Managers.Map.FindBeltFromBuilding(transform.position);
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    beltDir = (beltDir + 1) % 4;
                    if (beltItemIds[beltDir] != -1)
                    {
                        yield return new WaitUntil(() => nextBelt!=null && nextBelt.GetComponent<BuildingBase>().IsTransferAble(0));
                        nextBelt.GetComponent<BuildingBase>().SetBeltId(beltItemIds[beltDir]);
                        beltItemIds[beltDir] = -1;
                    }

                }
            }
            yield return new WaitForFixedUpdate();
        }

    }

}
