using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public string secondLevel;


    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(secondLevel);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }

}
