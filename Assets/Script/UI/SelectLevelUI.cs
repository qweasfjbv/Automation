using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelUI : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    private void Start()
    {
        buttons = transform.GetComponentsInChildren<Button>();
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();

        buttons[0].onClick.AddListener(() => OnNormalModeClicked());
        buttons[1].onClick.AddListener(() => OnHardModeClicked());
    }

    private void OnNormalModeClicked()
    {
        for(int i=1; i<Managers.Resource.GetBuildingCount(); i++)
        {
            Managers.Data.AddInvenItem(ResourceManager.BUILDINGOFFSET + i, 30);
        }
        Managers.Data.AddInvenItem(ResourceManager.BUILDINGOFFSET, 1000);
        
        gameObject.SetActive(false);
    }

    private void OnHardModeClicked()
    {
        gameObject.SetActive(false);
    }

}
