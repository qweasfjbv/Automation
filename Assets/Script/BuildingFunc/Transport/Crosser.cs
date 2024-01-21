using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Crosser : Transport
{

    int[] beltItemIds;
    [SerializeField]
    GameObject[] nextBelt;

    private void Start()
    {
        beltItemIds = new int[2] { -1, -1 };
        nextBelt = new GameObject[2] { null, null };

        StartCoroutine(CrossCoroutine());
    }


    public override void SetBeltId(int id, int rot = 0)
    {

        rot = (rot - Managers.Map.GetTileOnPoint(transform.position).rot + 4) % 4;

        beltItemIds[rot] = id;

    }

    public override bool IsTransferAble(int id, int rot)
    {
        rot = (rot - Managers.Map.GetTileOnPoint(transform.position).rot + 4)%4;

        return beltItemIds[rot] == -1;
    }

    private IEnumerator CrossCoroutine()
    {
        while (true)
        {

            for (int i = 0; i < 2; i++)
            {
                if (nextBelt[i] != null) continue;
                nextBelt[i] = Managers.Map.FindBeltFromBuilding(this, transform.position, i);
            }

            for (int i = 0; i < 2; i++)
            {
                if (nextBelt[i] == null || !nextBelt[i].GetComponent<Belt>().IsTransferAble(beltItemIds[i], i)) continue;
                else
                {
                    if (beltItemIds[i] != -1)
                    {
                        nextBelt[i].GetComponent<Belt>().SetBeltId(beltItemIds[i]);
                        beltItemIds[i] = -1;
                    }
                }
            }

            yield return new WaitForFixedUpdate();
        }



    }

    public override void EraseNextBelt(int rot)
    {
        nextBelt[(rot - Managers.Map.GetTileOnPoint(transform.position).rot + 4) % 4] = null;
    }
}
