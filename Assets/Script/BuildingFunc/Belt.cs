using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;

public class Belt : MonoBehaviour
{
    private static int _beltID;
    const int ID = 101;

    [SerializeField]
    int inDir, outDir;


    public void SetDirs(int dir)
    {
        inDir = dir;
        outDir = dir;
    }




    public Belt nextBelt;
    [SerializeField]
    private GameObject beltItem;
    private int beltItemId;
    public int BeltItemId { get => beltItemId; set => beltItemId = value; }

    private BeltManager beltManager;


    private Coroutine itemMoveCoroutine;

    private Vector3 startPos, endPos;


    void Start()
    {
        nextBelt = null;
        beltItemId = -1;
        SetDirs(Managers.Map.UsingArea[Mathf.Abs(Mathf.CeilToInt(transform.position.y)), Mathf.FloorToInt(transform.position.x)].rot);
        nextBelt = FindNextBelt();
        gameObject.name = $"Belt:{_beltID++}";
        itemMoveCoroutine = null;


    }

    private void Update()
    {

        if (nextBelt == null)
            nextBelt = FindNextBelt();

        if (beltItemId != -1 && itemMoveCoroutine == null)
        {
            beltItem.SetActive(true);
            itemMoveCoroutine = StartCoroutine(BeltMove(Managers.Resource.GetBuildingData(ID).Speed));
        }

        updateDir();

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
        float t = 0f;

        SetStartPos();
        SetEndPos();
        while (t < dl * 0.5f)
        {
            beltItem.transform.position = Vector3.Lerp(startPos, this.transform.position, t / (dl * 0.5f));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
        t = 0f;
        while (t < dl * 0.5f)
        {
            beltItem.transform.position = Vector3.Lerp(this.transform.position, endPos, t / (dl * 0.5f));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitUntil(() => nextBelt != null);


        beltItem.SetActive(false);
        nextBelt.BeltItemId = beltItemId;
        beltItemId = -1;

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

    private Belt FindNextBelt()
    {
        GameObject tmpBelt = Managers.Map.FindBelt(new Vector2(Mathf.Floor(transform.position.x), Mathf.Ceil(transform.position.y)), ID, ref outDir);
        if (tmpBelt == null) return null;

        SetEndPos();
        return tmpBelt.GetComponent<Belt>();
    }


}
