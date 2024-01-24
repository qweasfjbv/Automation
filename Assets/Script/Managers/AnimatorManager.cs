using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager
{
    private AnimatorStateInfo stateInfo;
    Dictionary<int, GameObject> crits = new Dictionary<int, GameObject>();

    public void Init()
    {
        crits.Add(101, GameObject.Find("BeltAnimCrit"));
        crits.Add(102, GameObject.Find("MMAnimCrit"));
        crits.Add(108, GameObject.Find("OilDrillAnimCrit"));
    }

    public int GetAnimId(int id)
    {
        GameObject obj;
        if (!crits.TryGetValue(id, out obj))
        {
            return -1;
        }

        return obj.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;

    }

    public float GetAnimTime(int id)
    {
        GameObject obj;
        if (!crits.TryGetValue(id, out obj))
        {
            return -1;
        }

        return obj.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;

    }

    public void Clear()
    {
        crits.Clear();
    }

}
