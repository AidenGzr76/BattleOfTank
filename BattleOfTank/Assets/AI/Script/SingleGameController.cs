using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleGameController : MonoBehaviour
{
    public GameObject singleStartMenu;
    public GameObject enemyNumberMenu;
    public GameObject singleEndMenu;

    public InputField enemyNumber;

    private void Start()
    {
        singleEndMenu.SetActive(false);
        enemyNumberMenu.SetActive(true);
        singleStartMenu.SetActive(false);
    }

    public void Confirm()
    {
        if (enemyNumber.text != null)
        {
            // Enemy Num
        }

        enemyNumberMenu.SetActive(false);
        singleStartMenu.SetActive(true);
    }

    public void singleAIStart()
    {
        // start
        
        singleStartMenu.SetActive(false);
    }

    public void Retry()
    {
        //retry

        singleEndMenu.SetActive(false);
        enemyNumberMenu.SetActive(false);
        singleStartMenu.SetActive(false);
    }

    public void LocalBack()
    {
        singleEndMenu.SetActive(false);
        enemyNumberMenu.SetActive(true);
        singleStartMenu.SetActive(false);
    }

    public void MainBack()
    {
        SceneManager.LoadScene("multi");
    }
}
