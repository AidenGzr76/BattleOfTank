using Pathfinding;
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

    public GameObject Enemy;
    public GameObject[] enemySpawnPos;

    int eNumber = 0;

    public GameObject nextpos;

    public Text enemyNumText;

    public static bool posDone = false;

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
            eNumber = int.Parse(enemyNumber.text);
        }
        else
        {
            eNumber = 5;
        }

        enemyNumText.text = eNumber.ToString();
        enemyNumberMenu.SetActive(false);
        singleStartMenu.SetActive(true);
    }

    public void singleAIStart()
    {
        int enemynumber = eNumber;

        for (int i = 0; i < eNumber; i++)
        {
            GameObject enemy = Instantiate(
                Enemy,
                enemySpawnPos[Random.Range(0, enemySpawnPos.Length)].transform.position,
                Quaternion.Euler(0, 0, Random.Range(0, 180))
                );

            enemynumber = i + 1;

            enemy.name = "Enemy" + "-" + enemynumber.ToString();

            GameObject npos = Instantiate(
                nextpos,
                new Vector3(Random.Range(-24, 24), Random.Range(-18, 10), 0),
                Quaternion.Euler(0, 0, Random.Range(0, 180))
                );

            npos.name = "nextPos" + "-" + enemynumber.ToString();

            AIDestinationSetter.posDone = true;
        }

        //AIDestinationSetter.posDone = true;
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
