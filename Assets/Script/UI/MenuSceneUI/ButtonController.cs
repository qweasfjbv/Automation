using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;

    private void Awake()
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(() => OnPressButtonTutorial());
        buttons[1].GetComponent<Button>().onClick.AddListener(() => OnPressButtonNewGame());
        buttons[2].GetComponent<Button>().onClick.AddListener(() => OnPressButtonContinue());
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnPressButtonSetting());
        buttons[4].GetComponent<Button>().onClick.AddListener(() => OnPressButtonExit());
    }

    private void OnPressButtonTutorial()
    {
        Managers.Scene.LoadScene(SceneEnum.Tutorial);
    }

    private void OnPressButtonNewGame()
    {
        Managers.Data.DeleteAll();
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonContinue()
    {
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonSetting()
    {

    }

    private void OnPressButtonExit()
    {

    }

}
