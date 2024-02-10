using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{

    private static EnvironmentManager instance;
    public static EnvironmentManager Instance { get { return instance; } }

    private int bdCnt = 0;
    private int purifierCnt = 0;
    private int pollutionMul = 1;
    private const int purifierOs = 5;

    private const int MaxValue = 100000;
    private float curValue = MaxValue;
    private float preValue = MaxValue;
    private float minValue = MaxValue;

    public float MinRate { get => minValue/MaxValue; }

    private Dictionary<int, Coroutine> fadeCoroutines = new Dictionary<int, Coroutine>();

    [SerializeField]
    private Slider envBar;
    [SerializeField]
    private Image envBarColor;
    [SerializeField]
    private List<GameObject> envTilemap;


    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Environment");
            if (go == null)
            {
                go = new GameObject { name = "@Environment" };
                go.AddComponent<EnvironmentManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<EnvironmentManager>();

        }
    }
    private void Awake()
    {
        Init();
        if (envBar != null) envBar.maxValue = MaxValue;
    }

    public void OnBuildBuilding()
    {
        bdCnt++;
    }
    public void OnUnbuildBuilding()
    {
        bdCnt--;
    }

    public void OnBuildPurifier()
    {
        purifierCnt++;
    }

    public void OnUnbuildPurifier()
    {
        purifierCnt--;
    }

    private void Update()
    {
        if (Managers.Scene.CurScene.GetComponent<GameScene>() == null) return;

        //curValue -= (pollutionMul * bdCnt - purifierCnt * purifierOs) / 100f;
        //envBar.value = curValue;

        curValue = envBar.value;

        if (envBar.value / envBar.maxValue > 0.5f)
        {
            envBarColor.color = Color.green;
        }
        else if (envBar.value / envBar.maxValue > 0.2f)
        {
            envBarColor.color = Color.yellow;
        }
        else
        {
            envBarColor.color = Color.red;
        }


        for (int i = 1; i <= envTilemap.Count; i++)
        {
            float critic = envBar.maxValue * (1 - ((float)i / (float)(envTilemap.Count + 1)));

            if((preValue >= critic) != (curValue >= critic))
            {
                if (pollutionMul * bdCnt - purifierCnt * purifierOs > 0)
                {
                    StartFadeOut(envTilemap.Count - i, 3.0f);
                }
                else
                {
                    StartFadeIn(envTilemap.Count - i, 3.0f);
                }
            }
        }

        preValue = curValue;
        minValue = Mathf.Min(curValue, minValue);
    }
    public void StartFadeIn(int tilemapId, float duration)
    {
        StartFade(tilemapId, duration, targetAlpha: 1);
    }

    // FadeOut 코루틴 시작
    public void StartFadeOut(int tilemapId, float duration)
    {
        StartFade(tilemapId, duration, targetAlpha: 0);
    }

    private void StartFade(int tilemapId, float duration, float targetAlpha)
    {
        if (fadeCoroutines.ContainsKey(tilemapId))
        {
            // 이미 진행 중인 코루틴이 있다면 중지
            StopCoroutine(fadeCoroutines[tilemapId]);
        }

        // 새 Fade 코루틴 시작
        fadeCoroutines[tilemapId] = StartCoroutine(FadeTilemap(tilemapId, duration, targetAlpha));
    }

    private IEnumerator FadeTilemap(int tilemapId, float duration, float targetAlpha)
    {

        Tilemap baseMap = envTilemap[tilemapId].transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap objectMap = envTilemap[tilemapId].transform.GetChild(1).GetComponent<Tilemap>();

        Color initialColor = baseMap.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, targetAlpha);

        float time = 0;
        while (time < duration)
        {
            baseMap.color = Color.Lerp(initialColor, targetColor, time / duration);
            objectMap.color = Color.Lerp(initialColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // 최종 알파값 설정
        baseMap.color = targetColor;
        objectMap.color = targetColor;
        fadeCoroutines.Remove(tilemapId);
    }

}
