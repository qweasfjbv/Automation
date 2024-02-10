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
    private Button OptionButton;
    [SerializeField]
    public GameObject OptionUI;
    [SerializeField]
    private Button ExitButton;

    private void Start()
    {
        QuestButton.onClick.RemoveAllListeners();
        OptionButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        QuestButton.onClick.AddListener(() => OnQuestButton());
        OptionButton.onClick.AddListener(() => OnOptionButton());
        ExitButton.onClick.AddListener(() => OnExitButton());

    }

    private void OnQuestButton()
    {

        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        QuestUI.SetActive(true);
    }
    private void OnOptionButton()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        OptionUI.GetComponent<OptionUI>().Toggle();
    }

    private void OnExitButton()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.SaveAll();
        Managers.Scene.LoadScene(SceneEnum.Mainmenu);
    }
}
