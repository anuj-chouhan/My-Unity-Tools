using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    private static AsyncOperation loadingAsyncOperation;
    private static Action onLoaderCallback;
    public enum Scenes
    {
        Loading,
        Gameplay,
    }

    public static void LoadScene(Scenes scene)
    {
        onLoaderCallback = () => 
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        // First, load the Loading screen
        SceneManager.LoadScene(Scenes.Loading.ToString());
    }

    // Check if there's a pending scene load request
    public static bool HasPendingLoad()
    {
        return onLoaderCallback != null;
    }

    private static IEnumerator LoadSceneAsync(Scenes scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperation.isDone)
        {
            yield return null ;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }

    // Execute the pending load request
    public static void LoaderCallBack()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}