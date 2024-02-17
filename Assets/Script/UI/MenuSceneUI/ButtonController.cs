using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++) buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();

        buttons[0].GetComponent<Button>().onClick.AddListener(() => OnPressButtonTutorial());
        buttons[1].GetComponent<Button>().onClick.AddListener(() => OnPressButtonNewGame());
        buttons[2].GetComponent<Button>().onClick.AddListener(() => OnPressButtonContinue());
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnpressButtonUpgrade());
        buttons[4].GetComponent<Button>().onClick.AddListener(() => OnPressButtonSetting());
        buttons[5].GetComponent<Button>().onClick.AddListener(() => OnPressButtonExit());
    }

    private void OnPressButtonTutorial()
    {

        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Scene.LoadScene(SceneEnum.Tutorial);
    }

    private void OnPressButtonNewGame()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.DeleteAll();
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonContinue()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonSetting()
    {

        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
    }

    private void OnPressButtonExit()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
    }

    private void OnpressButtonUpgrade()
    {

    }

}
