using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputModeUI : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> mods;
    [SerializeField]
    private List<GameObject> popups;

    private Dictionary<int, Transform> uiDict = new Dictionary<int, Transform>();

    private void Awake()
    {
        uiDict.Add(101, popups[0].transform.GetChild(0));
        uiDict.Add(103, popups[0].transform.GetChild(1));
        uiDict.Add(104, popups[0].transform.GetChild(2));
        uiDict.Add(105, popups[0].transform.GetChild(3));

        uiDict.Add(102, popups[1].transform.GetChild(0));
        uiDict.Add(108, popups[1].transform.GetChild(1));

        uiDict.Add(106, popups[2].transform.GetChild(0));
        uiDict.Add(109, popups[2].transform.GetChild(1));
        uiDict.Add(107, popups[2].transform.GetChild(2));
        uiDict.Add(110, popups[2].transform.GetChild(3));

        uiDict.Add(111, popups[5].transform.GetChild(0));



    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnModButtonClicked(1);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            OnModButtonClicked(2);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            OnModButtonClicked(3);
        }

        /*
        else if(Input.GetKeyDown(KeyCode.F4))
        {
            OnModButtonClicked(4);
        }
        else if(Input.GetKeyDown(KeyCode.F5))
        {
            OnModButtonClicked(5);
        }
        */
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            OnModButtonClicked(6);
        }


    }

    public void OnPrevIdChanged(int prevId, int id)
    {
        Transform tmp;

        if (!uiDict.TryGetValue(prevId, out tmp)) {
            Debug.Log("ERROR : prevId Doesn't exist");
            return; 
        }

        tmp.GetComponent<Image>().color = Color.clear;

        if (!uiDict.TryGetValue(id, out tmp))
        {
            Debug.Log("ERROR : id Doesn't exist");
            return;
        }

        tmp.GetComponent<Image>().color = Color.white;
        return;

    }

    public void OnModButtonClicked(int num)
    {
        int tmp = (int)Managers.Input.Mode;
        if (tmp==0)
        {
            popups[num-1].gameObject.SetActive(true);
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Managers.Input.Mode = (InputManager.InputMode)(num);

        }
        else if(tmp == num)
        {
            popups[num - 1].gameObject.SetActive(false);
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
            Managers.Input.Mode = 0;
        }
        else
        {
            popups[tmp - 1].gameObject.SetActive(false);
            popups[num - 1].gameObject.SetActive(true);
            mods[tmp-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Managers.Input.Mode = (InputManager.InputMode)(num);
        }

    }
}
