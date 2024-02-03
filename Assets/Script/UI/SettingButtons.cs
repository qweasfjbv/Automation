using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField]
    private Button QuestButton;
    [SerializeField]
    private Button OptionButton;
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

    }
    private void OnOptionButton()
    {

    }

    private void OnExitButton()
    {
        Managers.Data.SaveAll();
        Managers.Scene.LoadScene(SceneEnum.Mainmenu);
    }
}
