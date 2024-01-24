using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject buildPreviewer;
    [SerializeField]
    private GameObject buildingInfo;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float dragSpeed;
    [SerializeField]
    private float dragHoldTime;

    Vector2 minCameraPos = new Vector2(0, -100);
    Vector3 maxCameraPos = new Vector2(100, 0);

    private Camera thisCamera;
    private float scroll;
    private float targetSize;

    private readonly float minZoomSize = 3.0f;
    private readonly float maxZoomSize = 20.0f;

    private float onMouseTime = 0f;

    private Vector3 firstClickPoint;
    private Vector3 mousePosition;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        targetSize = thisCamera.orthographicSize;

        minCameraPos.y = -Managers.Map.MapSizeY;
        maxCameraPos.x = Managers.Map.MapSizeX;


        buildPreviewer.SetActive(false);
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        ZoomControl();
        UpdateZoom();

        if (Managers.Input.Mode == InputManager.InputMode.None) {

            buildPreviewer.SetActive(false);
            ViewMoving();
        }
        else
        {
            buildPreviewer.SetActive(true);
        }

    }

    private void ZoomControl()
    {
        var hasScrollInput = Mathf.Abs(scroll) > 0.1f;
        if (!hasScrollInput) return;

        var newSize = thisCamera.orthographicSize - scroll;

        targetSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);
    }

    private void UpdateZoom()
    {
        if (Mathf.Abs(targetSize - thisCamera.orthographicSize) < Mathf.Epsilon)
        {
            return;
        }
            
        var cameraTransform = transform;
        var currentCameraPosition = cameraTransform.position;
        var offsetCamera = mousePosition - currentCameraPosition - (mousePosition - currentCameraPosition) / (thisCamera.orthographicSize/targetSize);

        thisCamera.orthographicSize = targetSize;
            
        currentCameraPosition += offsetCamera;

        currentCameraPosition.x = Mathf.Clamp(currentCameraPosition.x, minCameraPos.x, maxCameraPos.x);
        currentCameraPosition.y = Mathf.Clamp(currentCameraPosition.y, minCameraPos.y, maxCameraPos.y);
        cameraTransform.position = currentCameraPosition;
    }
    void ViewMoving()
    {
        // 마우스 최초 클릭 시의 위치 기억
        if (Input.GetMouseButtonDown(0))
        {
            firstClickPoint = mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            onMouseTime += Time.deltaTime;
            if (onMouseTime > dragHoldTime)
            {
                Vector3 move = (-mousePosition + firstClickPoint) * dragSpeed;
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + move.x, minCameraPos.x, maxCameraPos.x)
                    , Mathf.Clamp(transform.position.y + move.y, minCameraPos.y, maxCameraPos.y), -10);
            }
        }
        else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (onMouseTime <= dragHoldTime)
            {
                var tmpTile = Managers.Map.GetTileOnPoint(mousePosition);
                if (tmpTile == null)
                {
                    buildingInfo.SetActive(false);
                }
                else if (tmpTile.id != -1)
                {
                    buildingInfo.GetComponent<BuildingInfo>().SetBuildingInfo(tmpTile.id, tmpTile.building);
                    buildingInfo.SetActive(true);
                }
                else
                {
                    buildingInfo.SetActive(false);
                }
            }
        }
        else
        {
            onMouseTime = 0;
        }
    }
}
