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
        // ShowModeSelection �� 1�̸� ������.
        // Ȯ���ϴ� �κ��� GameManagerEx�� ����
        PlayerPrefs.SetInt("ShowModeSelection", 1);
        SoundManager.Instance.PlaySfxSound(Define.SoundType.BUTTON1);
        Managers.Data.DeleteAll();
        Managers.Scene.LoadScene(SceneEnum.Game);
    }

    private void OnPressButtonContinue()
    {
        // ���� ������ �ε�
        // ������ NewGameȣ��
        if (!Managers.Data.IsThereMapData())
        {
            Debug.Log("����� �����Ͱ� ����. �� ���� ����");
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
