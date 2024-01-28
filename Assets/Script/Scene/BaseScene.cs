using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneEnum { 
    Unknown,
    Mainmenu,
    Tutorial,
    Game,
    Loading
};


public abstract class BaseScene : MonoBehaviour
{
    private SceneEnum scene = SceneEnum.Unknown;
    public SceneEnum Scene { get => scene; set => scene = value; }

    protected abstract void Init();
}
