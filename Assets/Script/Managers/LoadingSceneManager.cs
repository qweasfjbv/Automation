using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{

    [SerializeField] Slider slider;
    public static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);
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