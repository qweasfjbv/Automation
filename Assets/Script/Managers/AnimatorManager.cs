using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager
{
    private GameObject[] crits;
    private AnimatorStateInfo stateInfo;

    public void Init()
    {
        crits = new GameObject[5];
        crits[0] = GameObject.Find("BeltAnimCrit");
        crits[1] = GameObject.Find("MMAnimCrit");
        crits[2] = GameObject.Find("SmelterAnimCrit");
        crits[3] = GameObject.Find("OilDrillAnimCrit");
        crits[4] = GameObject.Find("RefineryAnimCrit");
    }

    public int GetAnimId(int id)
    {

        switch (id)
        {
            case 101:
                return crits[0].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;
            case 102:
                return crits[1].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;
            case 106:
                return crits[2].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;
            case 108:
                return crits[3].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;
            case 109:
                return crits[4].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash;

            default:
                break;
        }

        return -1;
    }

    public float GetAnimTime(int id)
    {
        switch (id) {

            case 101: 
                stateInfo = crits[0].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                break;

            case 102: 
                stateInfo = crits[1].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                break;

            case 106:
                stateInfo = crits[2].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                break;
            case 108:
                stateInfo = crits[3].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                break;
            case 109:
                stateInfo = crits[4].transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                break;

            default:
                break;
        }

        return stateInfo.normalizedTime;
    }

}
