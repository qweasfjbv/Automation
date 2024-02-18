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

        // ȭ���� ��踦 ���մϴ�.
        float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // ���콺 ��ġ�� ȭ�� ��� ���� �����մϴ�.
        float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);
        float clampedY = Mathf.Clamp(mousePos.y, minY, maxY);

        // GameObject�� ��ġ�� ���ѵ� ���콺 ��ġ�� �����մϴ�.
        transform.position = new Vector3(clampedX, clampedY, 0);

    }
}
