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
    private Vector3? lastPrevPoint;
    private Vector3 previewPoint;
    private Vector3 previewPosition;

    private Vector2 rotateOffset;

    private int rotateDir;

    public void Awake()
    {
        rotateDir = 0;
        sRenderer = preview.GetComponent<SpriteRenderer>();
        tmpC = sRenderer.color;

        rotateOffset = new Vector2(previewSize.x / 2 - 0.5f, -1 * previewSize.y / 2 + 0.5f);
    }

    private void RotatePreview()
    {
        float tmp = rotateOffset.y;
        rotateOffset.y = rotateOffset.x * -1;
        rotateOffset.x = tmp;

        rotateDir = (rotateDir + 1) % 4;
    }

    
    private void RotateToDir(int dir)
    {
        while (rotateDir != dir)
        {
            RotatePreview();
        }
    }

    public void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        previewPoint = new Vector3(Mathf.Floor(mousePosition.x), Mathf.Ceil(mousePosition.y), 0);

        previewPosition = new Vector3(previewPoint.x + 0.5f + rotateOffset.x, previewPoint.y - 0.5f + rotateOffset.y, 0);


        preview.transform.localScale = previewSize;
        preview.transform.position = previewPosition;
        preview.transform.rotation = Quaternion.Euler(0, 0, -90 * rotateDir);


        if (Managers.Map.BoundCheck(previewPoint, previewSize, rotateDir))
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
            if (lastPrevPoint != null)
            {
                if (lastPrevPoint.Value.x != previewPoint.x)
                {
                    if (lastPrevPoint.Value.x < previewPoint.x) RotateToDir(1);
                    else RotateToDir(3);
                }
                else if (lastPrevPoint.Value.y != previewPoint.y)
                {
                    if (lastPrevPoint.Value.y < previewPoint.y) RotateToDir(0);
                    else RotateToDir(2);
                }
            }

            Managers.Map.Build(11, previewPoint, previewSize, previewPosition, rotateDir);
            lastPrevPoint = previewPoint;
        }
        else
        {
            lastPrevPoint = null;
            if (Input.GetMouseButton(1))
            {
                Managers.Map.Unbuild(previewPoint);
            }
            else if (Input.GetKeyDown(Managers.Input.Rot))
            {
                RotatePreview();
            }
        }

    }
}
