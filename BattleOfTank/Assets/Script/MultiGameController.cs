using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiGameController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject multiStartMenu;
    public GameObject multiEndMenu;

    public static string PlayerName;

    private void Start()
    {
        multiEndMenu.SetActive(false);
        multiStartMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void SingleplayerBtn()
    {
        SceneManager.LoadScene("single");
    }

    public void MultiplayerBtn()
    {
        multiStartMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void Exit()
    {
        // exit
    }

    public void Back()
    {
        multiEndMenu.SetActive(false);
        multiStartMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void Retry()
    {

    }
}
