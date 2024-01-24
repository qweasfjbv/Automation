using TMPro;
using UnityEngine;

public class Managers : MonoBehaviour
{

    static Managers s_instance;
    static Managers Instance { get { return s_instance; } }


    MapManager _map = new MapManager();
    ResourceManager _resource = new ResourceManager();
    InputManager _input = new InputManager();
    AnimatorManager _anim = new AnimatorManager();
    PoolManager _pool = new PoolManager();
    SceneManagerEx _scene = new SceneManagerEx();

    public static MapManager Map { get { return Instance._map; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input { get { return Instance._input; } }
    public static AnimatorManager Anim { get {  return Instance._anim; } }
    public static PoolManager Pool { get { return Instance._pool; } }   
    public static SceneManagerEx Scene { get { return Instance._scene; } }

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
        s_instance._resource.Init();
        s_instance._anim.Init();
        s_instance._pool.Init();

    }

    public static void Clear()
    {
        s_instance._input.Clear();
        s_instance._pool.Clear();
        s_instance._map.Clear();
    }
}
