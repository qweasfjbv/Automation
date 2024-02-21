using TMPro;
using UnityEngine;

public class Managers : MonoBehaviour
{

    static Managers s_instance;
    public static Managers Instance { get { return s_instance; } }


    MapManager _map = new MapManager();
    ResourceManager _resource = new ResourceManager();
    InputManager _input = new InputManager();
    AnimatorManager _anim = new AnimatorManager();
    PoolManager _pool = new PoolManager();
    SceneManagerEx _scene = new SceneManagerEx();
    DataManager _data = new DataManager();
    QuestManager _quest = new QuestManager();

    public static MapManager Map { get { return Instance._map; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static InputManager Input { get { return Instance._input; } }
    public static AnimatorManager Anim { get { return Instance._anim; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static DataManager Data { get { return Instance._data; } }
    public static QuestManager Quest { get { return Instance._quest; } }

    void Awake()
    {
        Init();
    }
    private void Start()
    {
        SetResolution();
    }


    public void SetResolution()
    {
        int setWidth = 1920; 
        int setHeight = 1080; 

        Screen.SetResolution(setWidth, setHeight, true);
    }

    public static void ReInit() {

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


        s_instance._anim.Init();
        if (GameObject.FindObjectOfType<TutorialScene>() != null)
        {
            s_instance._map.Init(30);
            s_instance._pool.Init(100);
        }
        else if (GameObject.FindObjectOfType<GameScene>() != null)
        {
            s_instance._map.Init(100);
            s_instance._pool.Init(5000);
        }
        s_instance._data.Init();
        s_instance._quest.Init();
    }

    public void Init()
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
        else
        {
            Destroy(this.gameObject);
            return;
        }


        s_instance._anim.Init();
        s_instance._resource.Init();
        if (GameObject.FindObjectOfType<TutorialScene>() != null)
        {
            s_instance._map.Init(30);
            s_instance._pool.Init(100);
        }
        else if(GameObject.FindObjectOfType<GameScene>() != null)
        {
            s_instance._map.Init(100);
            s_instance._pool.Init(5000);
        }
        s_instance._data.Init();
        s_instance._quest.Init();

    }

    public static void Clear()
    {
        s_instance._input.Clear();
        s_instance._pool.Clear();
        s_instance._map.Clear();
        s_instance._anim.Clear();
    }
}
