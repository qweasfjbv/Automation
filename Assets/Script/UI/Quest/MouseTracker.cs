using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    void Start()
    {
        
    }

    Vector3 mousePos;

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // 화면의 경계를 구합니다.
        float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // 마우스 위치를 화면 경계 내로 제한합니다.
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);
        float clampedY = Mathf.Clamp(mousePos.y, minY, maxY);

        // GameObject의 위치를 제한된 마우스 위치로 설정합니다.
        transform.position = new Vector3(clampedX, clampedY, 0);

    }
}
