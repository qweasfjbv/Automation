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
    private GameObject finalReport;

    private const int MAXPOPULATION = 100000000;
    private float curPopulation = MAXPOPULATION;
    public readonly int PEOPLEPERSHIP = 500;

    const int MAXTRASH = 1000;
    public bool[] randomTable;

    public int MaxPopulation { get => MAXPOPULATION;  }
    public int SpaceShipCnt { get => spaceShipCnt; }
    public int CapsizedShipCnt { get => capsizedShipCnt; }
    public int TrashCnt { get => trashCnt;  }  
    public int CapsizedItemCnt { get => capsizedItemCnt;  }

    public QuestProgressDatas qpDatas;

    private void Start()
    {
        qpDatas = Managers.Data.GetQPDatas();
        randomTable = new bool[MAXTRASH];
        for (int i = 0; i < MAXTRASH; i++)
        {
            if (i % 2 == 0)
            {
                randomTable[i] = true;
                continue;
            }

            randomTable[i] = false;
        }

    }

    private bool isOver = false;

    private void Update()
    {
        curPopulation = MAXPOPULATION * EnvironmentManager.Instance.MinRate;

        // d
        if (curPopulation <= PEOPLEPERSHIP * spaceShipCnt && !isOver)
        {
            isOver = true;
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
        Managers.Input.KeyInputLock = true;
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        finalReport.SetActive(true);
        yield return null;
    }
}
