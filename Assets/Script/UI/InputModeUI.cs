using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModeUI : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> mods;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnModButtonClicked(1);
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            OnModButtonClicked(2);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            OnModButtonClicked(3);
        }
        else if(Input.GetKeyDown(KeyCode.F4))
        {
            OnModButtonClicked(4);
        }
        else if(Input.GetKeyDown(KeyCode.F5))
        {
            OnModButtonClicked(5);
        }
        else if(Input.GetKeyDown(KeyCode.F6))
        {
            OnModButtonClicked(6);
        }

    }

    public void OnModButtonClicked(int num)
    {
        int tmp = (int)Managers.Input.Mode;
        if (tmp==0)
        { 
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Managers.Input.Mode = (InputManager.InputMode)(num);

        }
        else if(tmp == num)
        {
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0f);
            Managers.Input.Mode = 0;
        }
        else
        {
            mods[tmp-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0f);
            mods[num-1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Managers.Input.Mode = (InputManager.InputMode)(num);
        }

    }
}
