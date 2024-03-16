using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour
{
    public GameObject difficulties;
    public GameObject mainMenu;

    public static SceneManager1 Instance;

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Easy()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Medium()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void Hard()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Anomaly()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Play() 
    {
        difficulties.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Back()
    {
        difficulties.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
