using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildPreviewer : MonoBehaviour
{
    [SerializeField] private GameObject preview;
    [SerializeField] private GameObject gridBound;
    [SerializeField] private List<GameObject> dirSprites;
    [SerializeField] private GameObject dirAxis;
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private InputModeUI inputModeUI;
    [SerializeField] private BuildingInfo buildingInfo;

    private int id;


    private Vector2 previewSize; 
    Color tmpC;


    private Vector3 mousePosition;
    private Vector3? lastPrevPoint;
    private Vector3 previewPoint;
    private Vector3 previewPosition;

    private Vector2 rotateOffset;

    private int rotateDir;
    private int prevId;
    

    private void Init()
    {

        prevId = id = 101;
        OnPrevBuildingChanged();
        rotateDir = 0;
        tmpC = sRenderer.color;
    }

    public void Start()
    {
        Init();
    }

    private void SettingById(int id)
    {
        sRenderer.sprite = Managers.Resource.GetBuildingSprite(id);
        previewSize = Managers.Resource.GetBuildingData(id).Size;
        
    }

    private void RotatePreview()
    {
        rotateOffset = new Vector2(previewSize.x / 2 - 0.5f, -1 * previewSize.y / 2 + 0.5f);
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

    public void SetPreviewerId(int id)
    {
        this.id = id;
    }

    private void GetButton()
    {

        switch (Managers.Input.Mode) {
            case InputManager.InputMode.F1:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    id = 101;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    id = 103;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    id = 104;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    id = 105;
                }
                break;
            case InputManager.InputMode.F2:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    id = 102;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    id = 108;
                }
                break;
            case InputManager.InputMode.F3:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    id = 106;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    id = 109;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    id = 107;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    id = 110;
                }
                break;
            case InputManager.InputMode.F6:
                if (Input.GetKeyDown(KeyCode.Alpha1)){
                    id = 111;
                }
                break;
            default:
                break;
        
        }

        if (prevId != id)
        {
            OnPrevBuildingChanged();
            prevId = id;
        }
    }

    private void OnPrevBuildingChanged()
    {
        for (int i = 0; i < 4; i++)
        {
            dirSprites[i].transform.GetChild(0).gameObject.SetActive(false);
            dirSprites[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        var tmp = Managers.Resource.GetBuildingData(id).OutputDirs;
        for(int i=0; i<tmp.Count; i++)
        {
            dirSprites[tmp[i]].transform.GetChild(0).gameObject.SetActive(true);
        }
        tmp = Managers.Resource.GetBuildingData(id).InputDirs;

        for (int i = 0; i < tmp.Count; i++)
        {
            dirSprites[tmp[i]].transform.GetChild(1).gameObject.SetActive(true);
        }

        inputModeUI.OnPrevIdChanged(prevId, id);
    }

    public bool MouseIsOnUI()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }

    int tmpDir = 0;
    public void Update()
    {
        SettingById(id);
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        previewPoint = new Vector3(Mathf.Floor(mousePosition.x), Mathf.Ceil(mousePosition.y), 0);

        previewPosition = new Vector3(previewPoint.x + 0.5f + rotateOffset.x, previewPoint.y - 0.5f + rotateOffset.y, 0);


        preview.transform.localScale = previewSize;
        preview.transform.position = previewPosition;
        // preview로 봤을때 스프라이트가 회전
        if (Managers.Resource.GetBuildingData(id).Prefab.GetComponent<Transport>() != null)
        {
            dirAxis.transform.localRotation = Quaternion.Euler(0, 0, 0);
            preview.transform.rotation = Quaternion.Euler(0, 0, -90 * rotateDir);
        }
        else
        {
            dirAxis.transform.localRotation = Quaternion.Euler(0, 0, -90 * rotateDir);
            preview.transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        if (Managers.Map.BoundCheck(previewPoint, previewSize, rotateDir))
        {
            tmpC.g = 1.0f;
            tmpC.b = 1.0f;
        }
        else
        {
            tmpC.b = 0.0f;
            tmpC.g = 0.0f;
        }
        sRenderer.color = tmpC;


        if (Input.GetMouseButton(0) && !MouseIsOnUI())
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                tmpDir = rotateDir;
                if (lastPrevPoint != null)
                {
                    if (lastPrevPoint.Value.x != previewPoint.x)
                    {
                        if (lastPrevPoint.Value.x < previewPoint.x) tmpDir = 1;
                        else tmpDir = 3;
                    }
                    else if (lastPrevPoint.Value.y != previewPoint.y)
                    {
                        if (lastPrevPoint.Value.y < previewPoint.y) tmpDir = 0;
                        else tmpDir = 2;
                    }

                    RotateToDir(tmpDir);

                }
            }

            Managers.Map.Build(id, previewPoint, previewSize, rotateDir);


            var tmpT = Managers.Map.GetTileOnPoint(previewPosition);

            if (tmpT != null && tmpT.id != -1 && tmpT.building.GetComponent<Belt>() != null)
            {
                tmpT.building.GetComponent<Belt>().SetOutdir(tmpDir);
            }

            lastPrevPoint = previewPoint;
        }
        else
        {
            lastPrevPoint = null;
            if (Input.GetMouseButton(1))
            {
                buildingInfo.gameObject.SetActive(false);
                Managers.Map.Unbuild(previewPoint);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePreview();
            }


            GetButton();
        }

    }


    private void OnDisable()
    {
        if (gridBound != null)
            gridBound.SetActive(false);
    }

    private void OnEnable()
    {
        gridBound.SetActive(true);
    }


}
