using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    [SerializeField] private GameObject viewportContent;
    [SerializeField] private Image helpImage;
    [SerializeField] private TextMeshProUGUI helpText;

    private Button[] buttonList;

    private int OnHelpUI = 0;

    private Color seletedColor = Color.blue;

    private void Start()
    {
        buttonList = viewportContent.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].onClick.RemoveAllListeners();
            int idx = i;
            buttonList[idx].onClick.AddListener(() => OnButtonClick(idx));
        }

        OnButtonClick(0);
        OnHelpUI = 0;
    }

    public void OnButtonClick(int id)
    {
        buttonList[OnHelpUI].gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        OnHelpUI = id;
        helpImage.sprite = Managers.Resource.GetHelpSprite(id);
        helpText.text = Managers.Resource.GetHelpContent(id);
        buttonList[OnHelpUI].gameObject.GetComponent<TextMeshProUGUI>().color = seletedColor;
    }


}
