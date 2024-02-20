using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField]
    public AudioClip[] audioClips;
    [SerializeField]
    public AudioClip[] factoryClips;
    [SerializeField]
    public AudioClip[] bgmClips;

    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;
    public AudioSource factoryPlayer;
    public AudioSource drillPlayer;

    private bool isFadeIn = false;
    private bool isFadeOut = false;
    private Coroutine soundFadeIn;
    private Coroutine soundFadeOut;




    private float sfxMaxVolume = 1f;
    private float bgmMaxVolume =1f;

    private void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Sound");
            if (go == null)
            {
                go = new GameObject { name = "@Sound" };
                go.AddComponent<SoundManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<SoundManager>();

        }
        else{
            Destroy(this.gameObject);
            return;
        }
    }

    private void Awake()
    {
        Init();

        bgmPlayer = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
        factoryPlayer = GameObject.Find("FactoryPlayer").GetComponent<AudioSource>();
        drillPlayer = GameObject.Find("DrillPlayer").GetComponent<AudioSource>();
    }

    public void SetBGMVolume(float volume)
    {
        bgmPlayer.volume = volume;
        bgmMaxVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxPlayer.volume = volume;
        factoryPlayer.volume = volume;
        drillPlayer.volume = volume;

        sfxMaxVolume = volume;
    }

    public void PlaySfxSound(Define.SoundType type)
    {
        int idx;
        switch (type)
        {
            case Define.SoundType.BUILD:
                idx = 0;
                break;
            case Define.SoundType.BUILDFAIL:
                idx = 1;
                break;
            case Define.SoundType.BUTTON1:
                idx = 2;
                break;
            case Define.SoundType.GETQUEST:
                idx = 3;
                break;
            case Define.SoundType.SUCCESS:
                idx = 4;
                break;
            case Define.SoundType.CHAT:
                idx = 5;
                break;
            default:
                idx = -1;
                break;
        }

        if (idx == -1) return;

        sfxPlayer.clip = audioClips[idx];
        sfxPlayer.Play();
    }
    public bool IsSfxPlaying()
    {
        return sfxPlayer.isPlaying;
    }

    public void PlayFactorySound(Define.FactoryType type)
    {
        int idx;
        switch (type)
        {
            case Define.FactoryType.AIR:
                idx = 0;
                break;
            case Define.FactoryType.ASSEMBLER:
                idx = 1;
                break;
            case Define.FactoryType.DRILL:
                idx = 2;
                break;
            case Define.FactoryType.REFINERY:
                idx = 3;
                break;
            case Define.FactoryType.SMELTER:
                idx = 4;
                break;
            case Define.FactoryType.TRANSPORT:
                idx = 5;
                break;
            case Define.FactoryType.WELDING:
                idx = 6;
                break;
            case Define.FactoryType.SLICE:
                idx = 7;
                break;
            case Define.FactoryType.MASSDRIVER:
                idx = 8;
                break;
            default:
                idx = -1;
                break;
        }

        if (idx == -1) return;

        factoryPlayer.clip = factoryClips[idx];
        if (isFadeIn)
        {
            StopCoroutine(soundFadeIn);
        }
        if (isFadeOut)
        {
            StopCoroutine(soundFadeOut);
            isFadeOut = false;
        }
        soundFadeIn = StartCoroutine(SoundFadeInCoroutine(factoryPlayer, 3));
    }

    public void StopFactorySound()
    {
        if (isFadeIn)
        {
            StopCoroutine(soundFadeIn);
            isFadeIn = false;
        }
        if (isFadeOut)
        {
            StopCoroutine(soundFadeOut);
        }
        soundFadeOut = StartCoroutine(SoundFadeOutCoroutine(factoryPlayer, 2));
    }

    public bool IsFactoryPlaying()
    {
        return factoryPlayer.isPlaying;
    }

    public void PlayDrillSound()
    {
        drillPlayer.clip = factoryClips[6];
        drillPlayer.Play();
    }
    public void StopDrillSound()
    {
        drillPlayer.Stop();
    }
    public bool IsDrillPlaying()
    {
        return drillPlayer.isPlaying;
    }

    public void ChangeBGM(Define.BgmType type)
    {
        int idx = -1;
        switch (type)
        {
            case Define.BgmType.MAIN:
                idx = 0;
                break;
            case Define.BgmType.GAME:
                idx = 1;
                break;
            case Define.BgmType.SPACE:
                idx = 2;
                break;
            default:
                idx = -1;
                break;
        }

        bgmPlayer.clip = bgmClips[idx];
        StartCoroutine(BGMFadeInCoroutine(bgmPlayer, 3.0f));
    }

    private IEnumerator SoundFadeInCoroutine(AudioSource player, float fadeTime)
    {
        isFadeIn = true;
        player.volume = 0f;
        player.Play();
        while (player.volume < sfxMaxVolume)
        {
            player.volume += sfxMaxVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        isFadeIn = true;
    }

    private IEnumerator BGMFadeInCoroutine(AudioSource player, float fadeTime)
    {
        player.volume = 0f;
        player.Play();
        while (player.volume < bgmMaxVolume)
        {
            player.volume += bgmMaxVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

    }

    private IEnumerator SoundFadeOutCoroutine(AudioSource player, float fadeTime)
    {
        isFadeOut = true;
        player.volume = sfxMaxVolume;

        while (player.volume > 0)
        {
            player.volume -= sfxMaxVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        player.Stop();

        isFadeOut = false;
    }

}
