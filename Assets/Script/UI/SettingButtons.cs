using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtons : MonoBehaviour
{
    [SerializeField]
    private Button OptionButton;
    [SerializeField]
    private Button ExitButton;

    private void Start()
    {
        OptionButton.onClick.AddListener(() => OnOptionButton());
        ExitButton.onClick.AddListener(() => OnExitButton());
    }

    private void OnOptionButton()
    {

    }

    private void OnExitButton()
    {
        // TODO : 저장하는 기능 필요
        Managers.Scene.LoadScene(SceneEnum.Mainmenu);
    }
}
