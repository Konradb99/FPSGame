using FPSMulti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Launcher launcher;
    public void JoinMatch()
    {
        //SceneManager.LoadScene("MainGameMap");
        launcher.Join();
    }
    public void CreateMatch()
    {
        launcher.Create();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
