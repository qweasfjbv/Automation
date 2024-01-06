using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildPreviewer : MonoBehaviour
{
    [SerializeField] private GameObject preview;
    [SerializeField] private Vector2 previewSize;

    private SpriteRenderer sRenderer;
    Color tmpC;


    private Vector3 mousePosition;
    private Vector3 previewPoint;
    private Vector3 previewPosition;


    public void Awake()
    {
        sRenderer = preview.GetComponent<SpriteRenderer>();
        tmpC = sRenderer.color;
    }


    public void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        previewPoint = new Vector3(Mathf.Floor(mousePosition.x), Mathf.Ceil(mousePosition.y), 0);
        previewPosition = new Vector3(previewPoint.x + previewSize.x / 2, previewPoint.y - previewSize.y / 2, 0);

        preview.transform.localScale = previewSize;
        preview.transform.position = previewPosition;


        if (Managers.Map.BoundCheck(previewPoint, previewSize))
        {
            tmpC.b = 1.0f;
            tmpC.r = 0.0f;
        }
        else
        {
            tmpC.b = 0.0f;
            tmpC.r = 1.0f;
        }
        sRenderer.color = tmpC;


        if (Input.GetMouseButton(0))
        {
            Managers.Map.Build(1, previewPoint, previewSize);
        }
        else if (Input.GetMouseButton(1))
        {
            Managers.Map.Unbuild(previewPoint);
        }


    }
}
