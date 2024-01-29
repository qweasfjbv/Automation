using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;

public class Belt : Transport
{

    private static int _beltID;
    const int ID = 101;
    int inDir, outDir;

    int prevOutDir;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject beltItem;

    [SerializeField]
    private GameObject scaleAxis;
    [SerializeField]
    private GameObject[] childs;

    private BuildingBase prevBuilding;

    public int BeltItemId { get => beltItemId; set => beltItemId = value; }
    public int OutDir { get => outDir; }

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

    private bool isWaitNextBelt = false;
    public bool IsWaitNextBelt { get => isWaitNextBelt; set => isWaitNextBelt = value; }

    private Vector3 startPos, endPos;

    public BuildingBase nextBelt;


    private Coroutine beltUpdateCoroutine;

    private void Init()
    {

        childs[0].SetActive(true);
        childs[0].GetComponent<Animator>().Play(Managers.Anim.GetAnimId(ID), 0, Managers.Anim.GetAnimTime(ID));
        childs[1].SetActive(false);

        beltItem.SetActive(false);
        nextBelt = null;
        SetDirs();
        prevOutDir = outDir;
        nextBelt = FindNextBelt();

        beltUpdateCoroutine = StartCoroutine(BeltUpdate());
    }



    void Start()
    {
        gameObject.name = $"Belt:{_beltID++}";
    }
    
    private IEnumerator BeltUpdate()
    {
        while (true)
        {

            if (beltItemId != -1 && beltItem.activeSelf == false)
            {
                
                StartCoroutine(BeltMove(1 / Managers.Resource.GetBuildingData(ID).Speed));
            }

            if (prevOutDir != outDir)
            {
                UpdateDir();
                prevOutDir = outDir;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }


    public override void SetBeltId(int id, int rot = 0)
    {
        if (id == -1) return;
        this.beltItemId = id;
        beltItem.GetComponent<SpriteRenderer>().sprite = Managers.Resource.GetItemSprite(id);
    }

    public override bool IsTransferAble(int id,int rot)
    {
        return beltItemId == -1 && beltItem.activeSelf == false;
    }

    private void UpdateDir()
    {
        beltItem.transform.rotation = Quaternion.identity;

        if ((outDir + 1) % 4 == inDir)
        {
            scaleAxis.transform.localScale = new Vector3(-1, 1, 1);
            childs[0].SetActive(false);
            childs[1].SetActive(true);
        }
        else if (outDir == inDir)
        {
            scaleAxis.transform.localScale = new Vector3(1, 1, 1);
            childs[1].SetActive(false);
            childs[0].SetActive(true);
            this.GetComponentInChildren<Animator>().Play(Managers.Anim.GetAnimId(ID), 0, Managers.Anim.GetAnimTime(ID));

        }
        else
        {
            scaleAxis.transform.localScale = new Vector3(1, 1, 1);
            childs[0].SetActive(false);
            childs[1].SetActive(true);
        }
    }

    private IEnumerator BeltMove(float dl)
    {
        beltItem.SetActive(true);

        if(prevBuilding.transform.GetComponent<Belt>() != null && prevBuilding.transform.GetComponent<Belt>().IsWaitNextBelt)
        {
            prevBuilding.transform.GetComponent<Belt>().IsWaitNextBelt = false;
            prevBuilding.GetComponent<Belt>().beltItem.SetActive(false);
        }
        
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

        yield return new WaitUntil(() => nextBelt != null && nextBelt.IsTransferAble(beltItemId, outDir));

        isWaitNextBelt = true;
        nextBelt.SetBeltId(beltItemId, outDir);
        beltItemId = -1;


        if (nextBelt != null && nextBelt.GetComponent<Belt>() == null)
        {
            beltItem.SetActive(false);
        }


        yield return null;
    }

    

    public void InvokeBelt()
    {
        if (nextBelt == null)
            nextBelt = FindNextBelt();
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
        nextBelt = FindNextBelt();
    }
    private BuildingBase FindNextBelt()
    {
        GameObject tmpBelt = Managers.Map.FindBuildingFromBelt(this, transform.position, ID, ref outDir);
        if (tmpBelt == null) return null;
        SetEndPos();
        return tmpBelt.GetComponent<BuildingBase>();
    }

    public void SetPrevBuilding(BuildingBase bbase)
    {
        prevBuilding = bbase;
    }

    private void OnDisable()
    {
        if (beltUpdateCoroutine != null)
            StopCoroutine(beltUpdateCoroutine);
        if (prevBuilding != null) prevBuilding.EraseNextBelt(inDir);
    }

    private void OnEnable()
    {
        beltItemId = -1;
        Init();
    }

    public override void EraseNextBelt(int rot)
    {
        nextBelt = null;
    }
}
