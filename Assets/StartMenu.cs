using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] string sceneName = "GameScene"; // set your scene name in Inspector

    // Must be public, non-static, and take no parameters for the Button list
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogError("StartMenu: sceneName is empty. Set it in the Inspector.");
    }

    // Optional if you add a Quit button later
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit called.");
    }
}
