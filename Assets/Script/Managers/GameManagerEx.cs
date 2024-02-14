using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
{

    private static GameManagerEx instance;
    public static GameManagerEx Instance { get { return instance; } }
    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManagerEx>();
            }

            instance = go.GetComponent<GameManagerEx>();

        }
    }
    private void Awake()
    {
        Init();
    }

    [SerializeField]
    private int spaceShipCnt = 0;
    [SerializeField]
    private int capsizedShipCnt = 0;
    [SerializeField]
    private int trashCnt = 0;
    [SerializeField]
    private int capsizedItemCnt = 0;

    [SerializeField]
    private GameObject fianlReport;

    const int MAXPOPULATION = 100000000;
    private float curPopulation = MAXPOPULATION;
    public readonly int PEOPLEPERSHIP = 500;

    const int MAXTRASH = 1000;
    public bool[] randomTable;

    public QuestProgressDatas qpDatas;

    private void Start()
    {
        qpDatas = Managers.Data.GetQPDatas();
        randomTable = new bool[MAXTRASH];
        for (int i = 0; i < MAXTRASH; i++)
        {
            randomTable[i] = false;
        }

    }

    private void Update()
    {
        curPopulation = MAXPOPULATION * EnvironmentManager.Instance.MinRate;

        if (curPopulation <= PEOPLEPERSHIP * spaceShipCnt)
        {
            OnGameOver(PEOPLEPERSHIP * spaceShipCnt);
        }

    }

    // true¸é Àüº¹
    public bool IsCapsized()
    {
        if (randomTable[Random.Range(0, MAXTRASH)])
        {
            capsizedItemCnt++;
            return true;
        }
        else{
            return false;
        }
    }

    public bool IsSpaceshipCapsized()
    {
        if(randomTable[Random.Range(0, MAXTRASH)])
        {
            capsizedShipCnt++;
            return true;
        }
        else{
            spaceShipCnt++;
            return false;
        }
    }

    public void ThrowItemsToSpace(int id)
    {
        if (trashCnt <= 999)
        {
            randomTable[trashCnt] = true;
            trashCnt++;
        }
        else
        {
            return;
        }
    }

    public void OnGameOver(int num)
    {
        Time.timeScale = 0f;
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            Debug.Log("GAMeover");
        }
    }
}
