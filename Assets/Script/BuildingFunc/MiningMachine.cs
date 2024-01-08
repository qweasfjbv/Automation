using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MiningMachine : MonoBehaviour
{
    [SerializeField] Belt nextBelt;


    private void Start()
    {
        nextBelt = null;

        //StartCoroutine(MiningCoroutine());
    }

    private void Update()
    {

        if (nextBelt == null)
        {
            GameObject tmpGo = Managers.Map.FindBeltFromMine(new Vector2(Mathf.Floor(transform.position.x), Mathf.Ceil(transform.position.y)));
            if (tmpGo != null) nextBelt = tmpGo.GetComponent<Belt>();
            else nextBelt = null;

        }

    }
    private IEnumerator MiningCoroutine()
    {
        while (true)
        {

        }

    }

}
