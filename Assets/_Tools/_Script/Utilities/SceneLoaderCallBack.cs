using UnityEngine;

namespace Anuj.Utility.SceneLoadSystem
{
    public class SceneLoaderCallBack : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;

                // Check if there is a pending scene load request
                if (SceneLoader.HasPendingLoad())
                {
                    SceneLoader.LoaderCallBack();
                }
                else
                {
                    // If no scene was specified, assume this is the first startup and load the main menu or default gameplay scene
                    SceneLoader.LoadScene(SceneLoader.Scenes.Gameplay);
                }
            }
        }
    }
}