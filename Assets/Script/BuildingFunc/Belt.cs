using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField]
    int inDir, outDir;

    Vector2 buildingPos;

    public void SetDirs(int dir)
    {
        inDir = dir;
        outDir = dir;
    }
    private static int _beltID;

    const int ID = 11;



    public Belt nextBelt;
    public BeltItem beltItem;

    private BeltManager beltManager;


    void Start()
    {
        nextBelt = null;
        SetDirs(Managers.Map.UsingArea[Mathf.Abs(Mathf.CeilToInt(transform.position.y)), Mathf.FloorToInt(transform.position.x)].rot);
        nextBelt = FindNextBelt();
        gameObject.name = $"Belt:{_beltID++}";
    }

    private void Update()
    {
        if (nextBelt == null)
            nextBelt = FindNextBelt();

        if (beltItem != null && beltItem.item != null)
            StartCoroutine(BeltMove());

        updateDir();
    }

    private void updateDir()
    {
        if ((outDir + 1) % 4 == inDir)
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.BeltSprites[2];
        }
        else if (outDir == inDir)
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.BeltSprites[0];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Managers.Resource.BeltSprites[1];
        }
    }

    public Vector3 GetItemPosition()
    {
        var padding = 0.3f;
        var position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    private IEnumerator BeltMove()
    {
        yield return null;
    }

    private Belt FindNextBelt()
    {
        GameObject tmpBelt = Managers.Map.FindBelt(new Vector2(Mathf.Floor(transform.position.x), Mathf.Ceil(transform.position.y)), ID, ref outDir);
        if (tmpBelt == null) return null;

        return tmpBelt.GetComponent<Belt>();
    }

}
