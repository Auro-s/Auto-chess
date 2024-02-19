using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void EnterTeam()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void EnterQuest()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
