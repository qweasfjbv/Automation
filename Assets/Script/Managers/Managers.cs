using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Managers : MonoBehaviour
{

    static Managers s_instance;
    static Managers Instance { get { return s_instance; } }


    MapManager _map = new MapManager();

    public static MapManager Map { get { return Instance._map; } }



    void Start()
    {
        Init();
    }


    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

        }

        s_instance._map.Init();
    }

}
