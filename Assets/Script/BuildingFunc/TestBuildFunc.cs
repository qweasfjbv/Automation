using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuildFunc : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(0, 0, 5 * Time.deltaTime);
    }

}
