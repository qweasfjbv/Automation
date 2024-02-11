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

            DontDestroyOnLoad(go);
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
    private int trashCnt = 0;

    const int MAXPOPULATION = 100000000;
    private float curPopulation = MAXPOPULATION;
    public readonly int PEOPLEPERSHIP = 500;

    public QuestProgressDatas qpDatas;

    private void Start()
    {
        qpDatas = Managers.Data.GetQPDatas();
    }

    private void Update()
    {
        curPopulation = MAXPOPULATION * EnvironmentManager.Instance.MinRate;

        if (curPopulation <= PEOPLEPERSHIP * spaceShipCnt)
        {
            OnGameOver(PEOPLEPERSHIP * spaceShipCnt);
        }

        //Debug.Log(curPopulation.ToString("N0"));
    }

    public void ThrowItemsToSpace(int id)
    {
        trashCnt++;
    }

    private void OnGameOver(int num)
    { 

    }

}
