using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public void Toggle()
    {
        if(gameObject.activeSelf) gameObject.SetActive(false);
        else gameObject.SetActive(true);    
    }

    private void Start()
    {
        gameObject.SetActive(false);

        // �����̴� �ʱⰪ ����
        bgmSlider.value = SoundManager.Instance.bgmPlayer.volume;
        sfxSlider.value = SoundManager.Instance.sfxPlayer.volume;

        // �����̴� �� ���� �̺�Ʈ�� �޼ҵ� ����
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }
}
