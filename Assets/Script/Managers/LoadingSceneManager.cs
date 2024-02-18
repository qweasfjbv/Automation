using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{

    [SerializeField] Slider slider;
    public static SceneEnum nextScene;

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    public static void LoadScene(SceneEnum sceneName)
    {
        nextScene = sceneName;

        switch (nextScene)
        {
            case SceneEnum.Mainmenu:
                SoundManager.Instance.ChangeBGM(Define.BgmType.MAIN);
                break;
            case SceneEnum.Game:
            case SceneEnum.Tutorial:
                SoundManager.Instance.ChangeBGM(Define.BgmType.GAME);
                break;
        }

        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(System.Enum.GetName(typeof(SceneEnum), nextScene));
        ao.allowSceneActivation = false;

        float elapsedTime = 0.0f;
        while (!ao.isDone)
        {
            yield return null;
            elapsedTime += Time.deltaTime;

            if (ao.progress >= 0.9f)
            {
                slider.value = Mathf.Lerp(slider.value, 1f, elapsedTime);

                if (slider.value == 1.0f)
                    ao.allowSceneActivation = true;
            }
            else
            {
                slider.value = Mathf.Lerp(slider.value, ao.progress, elapsedTime);
                if (slider.value >= ao.progress)
                {
                    elapsedTime = 0f;
                }
            }
        }


    }
}