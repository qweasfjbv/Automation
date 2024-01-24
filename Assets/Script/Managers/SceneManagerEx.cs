using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurScene { get => GameObject.FindObjectOfType<BaseScene>(); }

    public void LoadScene(SceneEnum scene)
    {
        Managers.Clear();
        SceneManager.LoadScene(System.Enum.GetName(typeof(SceneEnum), scene));
    }



}
