using System.Collections;
using System.Collections.Generic;
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

    private IEnumerator SplitCoroutine()
    {
        while (true)
        {
            for(int i =0; i<3; i++)
            {
                if (nextBelt[i] != null) continue;
                nextBelt[i] = Managers.Map.FindBeltFromBuilding(transform.position, (i + 4-NEXTOFFSET)%4);
            }

            if (this.beltItemId != -1)
            {

                for (int i = 0; i < 3; i++)
                {
                    beltDir = (beltDir + 1) % 3;
                    if (nextBelt[beltDir] == null || nextBelt[beltDir].GetComponent<Belt>().BeltItemId != -1) continue;
                    else
                    {
                        nextBelt[beltDir].GetComponent<Belt>().BeltItemId = this.beltItemId;
                        this.beltItemId = -1;
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
                }

            }
            yield return new WaitForFixedUpdate();
        }

    }

}

