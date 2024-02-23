using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;

    [SerializeField]
    GameObject upgradeUI;

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++) buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();

        buttons[0].GetComponent<Button>().onClick.AddListener(() => OnPressButtonNewGame());
        buttons[1].GetComponent<Button>().onClick.AddListener(() => OnPressButtonContinue());
        buttons[2].GetComponent<Button>().onClick.AddListener(() => OnpressButtonUpgrade());
        buttons[3].GetComponent<Button>().onClick.AddListener(() => OnPressButtonExit());
    }

    private void OnPressButtonNewGame()
    {
        // ShowModeSelection 이 1이면 보여줌.
        // 확인하는 부분은 GameManagerEx에 있음
        PlayerPrefs.SetInt("ShowModeSelection", 1);
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.DeleteAll();
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonContinue()
    {
        // 맵이 있으면 로드
        // 없으면 NewGame호출
        if (!Managers.Data.IsThereMapData())
        {
            Debug.Log("저장된 데이터가 없음. 새 게임 시작");
            OnPressButtonNewGame();
            return;
        }

        PlayerPrefs.SetInt("ShowModeSelection", 0);
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Scene.LoadScene(SceneEnum.Game);
    }
    private void OnPressButtonExit()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Application.Quit();
    }

    private void OnpressButtonUpgrade()
    {
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        upgradeUI.SetActive(true);

    }

}
