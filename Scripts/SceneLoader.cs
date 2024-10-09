using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneNumber) 
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(int sceneNumber)
    {
        StartCoroutine(WaitForSceneLoad(sceneNumber));
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(WaitForSceneLoad(sceneName));
    }
    public void LoadSceneAfterSeconds(int sceneNumber, float time)
    {
        StartCoroutine(WaitForSceneLoadAfterSeconds(sceneNumber, time));
    }

    public void LoadSceneAfterSeconds(string sceneName, float time)
    {
        StartCoroutine(WaitForSceneLoadAfterSeconds(sceneName, time));
    }

    private IEnumerator WaitForSceneLoad(string sceneName)
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return sceneLoadOperation;
        if (sceneLoadOperation.isDone)
        {
            LoadScene(sceneName);
        }
    }

    private IEnumerator WaitForSceneLoad(int sceneNumber)
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
        yield return sceneLoadOperation;
        if (sceneLoadOperation.isDone)
        {
            LoadScene(sceneNumber);
        }
    }

    private IEnumerator WaitForSceneLoadAfterSeconds(int sceneNumber, float time)
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);
        yield return sceneLoadOperation;
        yield return new WaitForSeconds(time);
        if (sceneLoadOperation.isDone)
        {
            LoadScene(sceneNumber);
        }
    }

    private IEnumerator WaitForSceneLoadAfterSeconds(string sceneName, float time)
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return sceneLoadOperation;
        yield return new WaitForSeconds(time);
        if (sceneLoadOperation.isDone)
        {
            LoadScene(sceneName);
        }
    }
}
