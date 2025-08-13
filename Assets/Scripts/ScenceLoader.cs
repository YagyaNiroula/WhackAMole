using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);  // Load main menu scene
    }
}
