using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
{

    private static GameManagerEx instance;
    public static GameManagerEx Instance { get { return instance; } }
    public static void Init()
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

    private int MAXPOPULATION = 100000000;
    private int curPopulation = 100000000;
    public readonly int PEOPLEPERSHIP = 100000;
    private int excessPopulation = 0;

    readonly int MAXTRASH = 1000;
    public bool[] randomTable;

    private int finalScore = 0;

    public int MaxPopulation { get => MAXPOPULATION;  }


    public int SpaceShipCnt { get => spaceShipCnt; }
    public int CapsizedShipCnt { get => capsizedShipCnt; }
    public int TrashCnt { get => trashCnt;  }  
    public int CapsizedItemCnt { get => capsizedItemCnt;  }
    
    // 위 네가지 SAVE/LOAD, 로드받고 randomtable 제작.

    // finalscore, curpopulation, excess는 계산하고 사용해서 ㄴ상관

    public int ExcessPopulation { get=> excessPopulation; }
    public int FinalScore { get => finalScore; }

    public QuestProgressData qpDatas;

    public void SetGameExData(GameExData data)
    {
        spaceShipCnt = data.spaceShipCnt;
        capsizedShipCnt = data.capsizedShipCnt;
        trashCnt = data.trashCnt;
        capsizedItemCnt = data.capsizedItemCnt;

    }

    public void QPDataLoad()
    {

        qpDatas = Managers.Data.GetQPDatas();

        Managers.Data.LoadGameExData();

        randomTable = new bool[MAXTRASH];
        for (int i = 0; i < MAXTRASH; i++)
        {
            if (i < trashCnt)
            {
                randomTable[i] = true; continue;
            }
            randomTable[i] = false;
        }

    }


    private bool isOver = false;

    private void Update()
    {
        curPopulation = (int)((float)MAXPOPULATION * EnvironmentManager.Instance.MinRate);

        // d
        if (curPopulation <= PEOPLEPERSHIP * spaceShipCnt && !isOver)
        {
            isOver = true;
            OnGameOver(PEOPLEPERSHIP * spaceShipCnt);
        }

    }

    // true면 전복
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

        // 끝나기전에 계산전부 해야함
        excessPopulation = 0;
        for(int i=0; i< Managers.Data.GetQPDatas().populations.Length; i++) {
            excessPopulation += Mathf.Max(0,( Managers.Data.GetQPDatas().populations[i]- Managers.Resource.GetQuestData(i).PopulationLimit));
        }

        /*
        trashCnt = 126;
        capsizedShipCnt = 80;
        capsizedItemCnt = 62;

        spaceShipCnt = 880;
        excessPopulation = 3100000;
        */


        finalScore = Mathf.RoundToInt((spaceShipCnt * PEOPLEPERSHIP - (float)excessPopulation/2)/MAXPOPULATION * (0.5f + (1-trashCnt/(float)MAXTRASH) * 100));
        
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        finalReport.SetActive(true);
        yield return null;
    }
}
