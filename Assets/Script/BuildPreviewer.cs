using UnityEngine;
using UnityEngine.EventSystems;

public class BuildPreviewer : MonoBehaviour
{
    [SerializeField] private GameObject preview;


    private int id;


    private Vector2 previewSize;
    private SpriteRenderer sRenderer;
    Color tmpC;


    private Vector3 mousePosition;
    private Vector3? lastPrevPoint;
    private Vector3 previewPoint;
    private Vector3 previewPosition;

    private Vector2 rotateOffset;

    private int rotateDir;

    private void Init()
    {
        id = 101;
        rotateDir = 0;
        sRenderer = preview.GetComponent<SpriteRenderer>();
        tmpC = sRenderer.color;
    }

    public void Awake()
    {
        Init();
    }

    private void SettingById(int id)
    {
        if (id == 101)
        {
            sRenderer.sprite = Managers.Resource.GetBeltSprite(0);
        }
        else
        {
            sRenderer.sprite = Managers.Resource.GetBuildingSprite(id);
        }
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
                break;
            case InputManager.InputMode.F3:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    id = 106;
                }
                break;
            default:
                break;
        
        }

    }

    RaycastHit2D hit;
    private bool CanClickToBuild()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        return true;
    }

    public void Update()
    {
        SettingById(id);
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        previewPoint = new Vector3(Mathf.Floor(mousePosition.x), Mathf.Ceil(mousePosition.y), 0);

        previewPosition = new Vector3(previewPoint.x + 0.5f + rotateOffset.x, previewPoint.y - 0.5f + rotateOffset.y, 0);


        preview.transform.localScale = previewSize;
        preview.transform.position = previewPosition;
        preview.transform.rotation = Quaternion.Euler(0, 0, -90 * rotateDir);


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

        if (Input.GetMouseButton(0) && CanClickToBuild())
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

            Managers.Map.Build(id, previewPoint, previewSize, previewPosition, rotateDir);
            lastPrevPoint = previewPoint;
        }
        else
        {
            lastPrevPoint = null;
            if (Input.GetMouseButton(1))
            {
                Managers.Map.Unbuild(previewPoint);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePreview();
            }


            GetButton();
        }

    }





}
