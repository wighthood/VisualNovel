using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game scene");
    }
}

