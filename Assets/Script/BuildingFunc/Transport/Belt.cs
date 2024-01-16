using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;

public class Belt : BuildingBase
{

    private static int _beltID;
    const int ID = 101;

    int inDir, outDir;

    [SerializeField]
    private GameObject beltItem;

    public void SetDirs(int dir)
    {
        beltItem.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90*inDir));
        inDir = dir;
        outDir = dir;
    }




    private Coroutine itemMoveCoroutine;

    private Vector3 startPos, endPos;
    int rem;

    public BuildingBase nextBuilding;

    private void Init()
    {

        nextBuilding = null;
        beltItemId = -1;
        SetDirs(Managers.Map.UsingArea[Mathf.Abs(Mathf.CeilToInt(transform.position.y)), Mathf.FloorToInt(transform.position.x)].rot);
        nextBuilding = FindNextBelt();
        rem = _beltID;
        gameObject.name = $"Belt:{_beltID++}";
        itemMoveCoroutine = null;

    }
    void Start()
    {
        Init();
    }

    private void Update()
    {

        if (nextBuilding == null)
            nextBuilding = FindNextBelt();


        if (beltItemId != -1 && beltItem.activeSelf == false)
        {

            beltItem.GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetItemSprite(beltItemId);
            itemMoveCoroutine = StartCoroutine(BeltMove(Managers.Resource.GetBuildingData(ID).Speed));
        }

        updateDir();

    }

    public override void SetBeltId(int id, int rot = 0)
    {
        this.beltItemId = id;
    }

    public override bool IsTransferAble(int id,int rot)
    {
        return beltItemId == -1;
    }

    private void updateDir()
    {
        if ((outDir + 1) % 4 == inDir)
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetBeltSprite(2);
        }
        else if (outDir == inDir)
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetBeltSprite(0);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetBeltSprite(1);
        }
    }

    private IEnumerator BeltMove(float dl)
    {
        beltItem.SetActive(true);
        float t = 0f;

        SetStartPos();
        SetEndPos();
        beltItem.transform.position = startPos;

        while (t < dl * 0.5f)
        {
            beltItem.transform.position = Vector3.Lerp(startPos, this.transform.position, t / (dl * 0.5f));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        t = 0f;
        while (t < dl * 0.5f)
        {
            beltItem.transform.position = Vector3.Lerp(this.transform.position, endPos, t / (dl * 0.5f));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitUntil(() => nextBuilding != null && nextBuilding.IsTransferAble(beltItemId, outDir));

        nextBuilding.SetBeltId(beltItemId, outDir);
        beltItemId = -1;

        if(nextBuilding.GetComponent<Belt>() != null)
        {
            yield return new WaitUntil(() => nextBuilding != null && nextBuilding.GetComponent<Belt>().beltItem.activeSelf == true);
        }

        beltItem.SetActive(false);

        yield return null;

    }

    float[] by = {0.5f, 0, -0.5f, 0 };
    float[] bx = {0, 0.5f, 0, -0.5f};
    private void SetStartPos()
    {
        startPos = new Vector3(transform.position.x + -1 * bx[inDir] *Managers.Resource.GetBuildingData(ID).Size.x, transform.position.y + -1 * by[inDir]*Managers.Resource.GetBuildingData(ID).Size.y, transform.position.z);
    }
    private void SetEndPos()
    {
        endPos = new Vector3(transform.position.x + bx[outDir] * Managers.Resource.GetBuildingData(ID).Size.x, transform.position.y + by[outDir] * Managers.Resource.GetBuildingData(ID).Size.y, transform.position.z);
    }

    public void SetOutdir(int dir)
    {
        outDir = dir;
        FindNextBelt();
    }
    private BuildingBase FindNextBelt()
    {
        GameObject tmpBelt = Managers.Map.FindBuildingFromBelt(transform.position, ID, ref outDir);
        if (tmpBelt == null) return null;

        SetEndPos();
        return tmpBelt.GetComponent<BuildingBase>();
    }


}
