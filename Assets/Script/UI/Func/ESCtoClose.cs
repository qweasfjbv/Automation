using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCtoClose : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
