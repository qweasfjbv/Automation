using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private KeyCode rot;

    public void Init()
    {
        rot = KeyCode.R;
    }

    public KeyCode Rot { get => rot; }

}
