using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName) //Load a scene with a given name
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene() //Restart the current scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
