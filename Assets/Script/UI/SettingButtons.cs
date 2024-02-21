using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField]
    private Button QuestButton;
    [SerializeField]
    public GameObject QuestUI;
    [SerializeField]
    private Button SaveButton;
    [SerializeField]
    private GameObject SaveUI;
    [SerializeField]
    private Button OptionButton;
    [SerializeField]
    public GameObject OptionUI;
    [SerializeField]
    private Button HelpButton;
    [SerializeField] 
    private GameObject HelpUI;

    [SerializeField]
    private Button ExitButton;

    private void Start()
    {
        QuestButton.onClick.RemoveAllListeners();
        SaveButton.onClick.RemoveAllListeners();
        OptionButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
        HelpButton.onClick.RemoveAllListeners();

        SaveUI.GetComponent<Button>().onClick.RemoveAllListeners();

        QuestButton.onClick.AddListener(() => OnQuestButton());
        OptionButton.onClick.AddListener(() => OnOptionButton());
        ExitButton.onClick.AddListener(() => OnExitButton());
        SaveButton.onClick.AddListener(() => OnSaveButton());
        HelpButton.onClick.AddListener(() => OnHelpButton());

        SaveUI.GetComponent<Button>().onClick.AddListener(() => SaveUI.SetActive(false));

    }

    private void OnQuestButton()
    {

        HelpUI.SetActive(false);

        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        QuestUI.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnOptionButton()
    {
        HelpUI.SetActive(false);

        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        OptionUI.GetComponent<OptionUI>().Toggle();
    }

    private void OnExitButton()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.SaveAll();
        Managers.Scene.LoadScene(SceneEnum.Mainmenu);
    }

    private void OnSaveButton()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.SaveAll();
        SaveUI.SetActive(true);
    }

    private void OnHelpButton()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        if (HelpUI.activeSelf)
        {
            HelpUI.SetActive(false);
        }
        else
        {
            HelpUI.SetActive(true);
        }
    }


}
