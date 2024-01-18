using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Splitter : BuildingBase
{
    // 출구가 0,1,3번임. 1더해서 1,2,0 으로 만들고 배열관리
    readonly int NEXTOFFSET = 1;

    [SerializeField]
    private GameObject[] nextBelt;
    private int beltDir;

    private void Init()
    {
        beltItemId = -1;
        nextBelt = new GameObject[3];
        beltDir = 0;
    }

    private void Start()
    {
        Init();
        StartCoroutine(SplitCoroutine());
    }

    public override void SetBeltId(int id, int rot = 0)
    {
        this.beltItemId = id;
    }
    public override bool IsTransferAble(int id,int rot)
    {
        return beltItemId == -1;
    }

    private IEnumerator SplitCoroutine()
    {
        while (true)
        {
            for(int i =0; i<3; i++)
            {
                if (nextBelt[i] != null) continue;
                nextBelt[i] = Managers.Map.FindBeltFromBuilding(this, transform.position, (i + 4-NEXTOFFSET)%4);
            }

            if (this.beltItemId != -1)
            {

                for (int i = 0; i < 3; i++)
                {
                    beltDir = (beltDir + 1) % 3;
                    if (nextBelt[beltDir] == null || !nextBelt[beltDir].GetComponent<Belt>().IsTransferAble(beltItemId, beltDir)) continue;
                    else
                    {
                        nextBelt[beltDir].GetComponent<Belt>().SetBeltId(this.beltItemId);
                        this.beltItemId = -1;
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
                }

            }
            yield return new WaitForFixedUpdate();
        }

    }

    public override void EraseNextBelt(int rot)
    {
        nextBelt[(rot - Managers.Map.GetTileOnPoint(transform.position).rot + NEXTOFFSET + 4) % 4] = null;
    }
}

