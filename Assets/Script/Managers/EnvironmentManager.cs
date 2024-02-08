using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{

    private static EnvironmentManager instance;
    public static EnvironmentManager Instance { get { return instance; } }

    private int bdCnt = 0;
    private int purifierCnt = 0;
    private int pollutionMul = 1;
    private const int purifierOs = 5;


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
        envBar.value -= (pollutionMul * bdCnt - purifierCnt * purifierOs) / 100f;

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

        for (int i = 0; i < envTilemap.Count; i++)
        {
            if(envBar.value / envBar.maxValue >= (envTilemap.Count-i) / (float)envTilemap.Count)
            {
                envTilemap[i].SetActive(true);
            }
            else
            {
                envTilemap[i].SetActive(false);
            }
        }
    }


}
