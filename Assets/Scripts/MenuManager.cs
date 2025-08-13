
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel;
    public void StartGame() => SceneManager.LoadScene(1);
    public void ShowInstructions() => SceneManager.LoadScene(2);
    public void HideInstructions() => instructionsPanel.SetActive(false);
    public void ExitGame() => Application.Quit();
}
