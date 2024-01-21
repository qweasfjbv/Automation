using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputModePopupUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildings;

    [SerializeField]
    private BuildPreviewer previewer;



    public void SetPreviewId(int id)
    {
        previewer.SetPreviewerId(id);
    }

}
