using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public enum InputMode
    {
        None = 0,
        F1, F2, F3, F4, F5, F6
    }

    private InputMode inputMode = InputMode.None;

    public InputMode Mode { get => inputMode; set => inputMode = value; }

    public void Clear()
    {
        inputMode = InputMode.None;
    }

    private bool keyInputLock = false;
    public bool KeyInputLock { get => keyInputLock; set => keyInputLock = value; }
}
