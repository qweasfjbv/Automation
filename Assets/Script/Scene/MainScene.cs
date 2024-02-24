using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    [SerializeField]
    private Texture2D defaultMouse;
    protected override void Init()
    {
        Scene = SceneEnum.Mainmenu;
    }

    private void Start()
    {
        Cursor.SetCursor(defaultMouse, Vector2.zero, CursorMode.ForceSoftware);
    }
}
