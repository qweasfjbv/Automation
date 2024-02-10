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

        // 슬라이더 초기값 설정
        bgmSlider.value = SoundManager.Instance.bgmPlayer.volume;
        sfxSlider.value = SoundManager.Instance.sfxPlayer.volume;

        // 슬라이더 값 변경 이벤트에 메소드 연결
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }
}
