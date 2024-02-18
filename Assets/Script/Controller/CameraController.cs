using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEngine.TerrainTools.PaintContext;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject buildPreviewer;
    [SerializeField]
    private GameObject buildingInfo;
    [SerializeField]
    private InventoryUI inven;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float dragSpeed;
    [SerializeField]
    private float dragHoldTime;
    [SerializeField]
    private Texture2D defaultMouse;
    [SerializeField]
    private Texture2D drillMouse;
    [SerializeField]
    private Texture2D hammerMouse;
    [SerializeField]
    private ParticleSystem dustParticle;

    private bool isParticleOn;

    Vector2 minCameraPos = new Vector2(0, -100);
    Vector3 maxCameraPos = new Vector2(100, 0);

    private Camera thisCamera;
    private float scroll;
    private float targetSize;

    private readonly float minZoomSize = 3.0f;
    private readonly float maxZoomSize = 20.0f;

    private float onMouseTime = 0f;
    private float onMouseTimeR = 0f;

    private Vector3 firstClickPoint;
    private Vector3 mousePosition;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        targetSize = thisCamera.orthographicSize;

        minCameraPos.y = -Managers.Map.MapSizeY;
        maxCameraPos.x = Managers.Map.MapSizeX;

        isParticleOn = false;
        buildPreviewer.SetActive(false);

        dustParticle.Stop();
    }

    private float moveSpeed = 7f;
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
           ZoomControl();
            UpdateZoom();
        }


        if (Managers.Input.Mode == InputManager.InputMode.None) {

            buildPreviewer.SetActive(false);
            OnMouseClickEvent();
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.SetCursor(defaultMouse, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(hammerMouse, new Vector2(0, hammerMouse.height / 2), CursorMode.ForceSoftware);
            }
            buildPreviewer.SetActive(true);
            OffParticle();
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
    private void OnMouseClickEvent()
    {
        bool IsOnUI = EventSystem.current.IsPointerOverGameObject();

        // 마우스 최초 클릭 시의 위치 기억

        if (Input.GetMouseButtonDown(0) && !IsOnUI)
        {
            firstClickPoint = mousePosition;
        }

        if (Input.GetMouseButton(0) && !IsOnUI)
        {
            onMouseTime += Time.deltaTime;
            if (onMouseTime > dragHoldTime)
            {
                Vector3 move = (-mousePosition + firstClickPoint) * dragSpeed;
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + move.x, minCameraPos.x, maxCameraPos.x)
                    , Mathf.Clamp(transform.position.y + move.y, minCameraPos.y, maxCameraPos.y), -10);
            }
        }
        else if (Input.GetMouseButtonUp(0) && !IsOnUI)
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
        else if (Input.GetMouseButton(1) && !IsOnUI)
        {
            var tile = Managers.Map.GetTileOnPoint(mousePosition);
            dustParticle.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            

            if (tile != null && tile.terrainInfo>= 1 && tile.terrainInfo <= 6)
            {
                if (!SoundManager.Instance.IsDrillPlaying())
                {
                    SoundManager.Instance.PlayDrillSound();
                }
                Cursor.SetCursor(drillMouse, new Vector2(0, drillMouse.height), CursorMode.ForceSoftware);
                if (!isParticleOn)
                {
                    OnParticle();
                }
                onMouseTimeR += Time.deltaTime;

                if (onMouseTimeR >= Managers.Resource.GetItemData(Managers.Resource.GetTerrainData(tile.terrainInfo).OreID).ProductTime)
                {
                    onMouseTimeR -= Managers.Resource.GetItemData(Managers.Resource.GetTerrainData(tile.terrainInfo).OreID).ProductTime;
                    inven.OnGetItem(Managers.Resource.GetTerrainData(tile.terrainInfo).OreID, 1);
                }

            }

        }
        else
        {

            if (SoundManager.Instance.IsDrillPlaying())
            {
                SoundManager.Instance.StopDrillSound();
            }
            Cursor.SetCursor(defaultMouse, Vector2.zero, CursorMode.ForceSoftware);

            OffParticle();
            onMouseTimeR = 0;
            onMouseTime = 0;
        }
    }

    private void OffParticle()
    {
        dustParticle.Stop();
        isParticleOn = false;
    }

    private void OnParticle()
    {
        dustParticle.Play();
        isParticleOn = false;
    }
}
