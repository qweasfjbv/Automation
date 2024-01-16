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

    [SerializeField]
    int inDir, outDir;

    [SerializeField]
    private GameObject beltItem;

    public void SetDirs()
    {
        float tmp = transform.eulerAngles.z;
        while (tmp <= -10)
        {
            tmp += 360;
        }

        int dir;

        if (tmp <= 45)
            dir = 0;
        else if (tmp <= 135)
            dir = 3;
        else if (tmp <= 225)
            dir = 2;
        else
            dir = 1;

        inDir = dir;
        outDir = dir;
        beltItem.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90 * inDir));
    }




    private Vector3 startPos, endPos;

    public BuildingBase nextBuilding;

    private void Init()
    {
        nextBuilding = null;
        beltItemId = -1;
        SetDirs();
        nextBuilding = FindNextBelt();

    }



    void Start()
    {
        Init();
        gameObject.name = $"Belt:{_beltID++}";
    }

    private void Update()
    {

        if (nextBuilding == null)
            nextBuilding = FindNextBelt();


        if (beltItemId != -1 && beltItem.activeSelf == false)
        {

            beltItem.GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetItemSprite(beltItemId);
            StartCoroutine(BeltMove(1/Managers.Resource.GetBuildingData(ID).Speed));
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
        nextBuilding = FindNextBelt();
    }
    private BuildingBase FindNextBelt()
    {
        GameObject tmpBelt = Managers.Map.FindBuildingFromBelt(transform.position, ID, ref outDir);
        if (tmpBelt == null) return null;
        SetEndPos();
        return tmpBelt.GetComponent<BuildingBase>();
    }

    private void OnEnable()
    {
        Init();
    }
}
