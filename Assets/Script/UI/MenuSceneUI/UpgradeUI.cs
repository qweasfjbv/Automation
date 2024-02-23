using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI biasText;
    [SerializeField] private GameObject[] upgrades;


    void Start()
    {
        UpgradeSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.Data.SaveUpgradeData();
            gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Managers.Data.AddBias(100);
            biasText.text = "B " + Managers.Data.GetBias().ToString();

        }
    }

    private void UpgradeSetting()
    {

        biasText.text = "B " + Managers.Data.GetBias().ToString();
        for (int i = 0; i < 3; i++)
        {
            UpgradeFloorSetting(i);
        }



    }

    private void UpgradeFloorSetting(int id)
    {
        Image[] bars = upgrades[id].GetComponentsInChildren<Image>();
        for (int i = 0; i < Managers.Resource.GetUpgradeFloorCnt(id); i++)
        {
            if (i < Managers.Data.GetUpgradeFloor(id)) bars[i].color = Color.green;
            else bars[i].color = Color.white;
        }
        TextMeshProUGUI[] texts = upgrades[id].GetComponentsInChildren<TextMeshProUGUI>();

        if (Managers.Data.GetUpgradeFloor(id) != 0)
        {
            texts[1].text = "X " + Managers.Resource.GetUpgradeValue(id, Managers.Data.GetUpgradeFloor(id) - 1).ToString("N1");
        }
        else
        {
            texts[1].text = "X 1.0";
        }

        Button btn = upgrades[id].GetComponentInChildren<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Managers.Data.UpgradeRequest(id));
        btn.onClick.AddListener(() => UpgradeSetting());
        btn.onClick.AddListener(() => SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1));

        var tmpCost = Managers.Resource.GetUpgradeCost(id, Managers.Data.GetUpgradeFloor(id));
        if (tmpCost < 0)
        {
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Done";
        }
        else
        {
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "B + " + Managers.Resource.GetUpgradeCost(id, Managers.Data.GetUpgradeFloor(id)).ToString();
        }
    }

}
