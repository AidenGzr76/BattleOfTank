﻿using Pathfinding;
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
    public Text playerScore;

    public static int globalEnemyNumber = 3;

    public GameObject MainCamera;

    public Text Kills;

    private AudioSource menuSound;
    private AudioSource gameSound;

    public static int wave = 1;
    public Text waveText;

    private void Start()
    {
        menuSound = GameObject.Find("menuSound").GetComponent<AudioSource>();
        gameSound = GameObject.Find("gameSound").GetComponent<AudioSource>();

        menuSound.Play();

        singleEndMenu.SetActive(false);
        enemyNumberMenu.SetActive(true);
        singleStartMenu.SetActive(false);
        Time.timeScale = 0;
    }

    int enemyNumberToShow = 0;
    public static bool levelUp = false;

    public void FixedUpdate()
    {
        Kills.text = AIHealth.score + "/" + enemyNumberToShow;
        waveText.text = wave.ToString();

        if (AIHealth.destroyPlayer)
        {
            MainCamera.SetActive(true);

            singleEndMenu.SetActive(true);
            playerScore.text = AIHealth.score.ToString();
            AIHealth.score = 0;
            Time.timeScale = 0;

            AIHealth.destroyPlayer = false;
        }

        if (AIHealth.score == enemyNumberToShow) 
        {
            PlayerFind.playerFinded = false;
            PlayerBullet.playerShot = false;
            AIHealth.isEscape = false;
            //AIHealth.score = 0;
            AIDestinationSetter.wantSearch = false;

            GunPlayerFind.GunFindedplayer = false;
            PlayerBullet.playerShotToGun = false;

            wave++;
            int tempFirsteNumber = eNumber;
            eNumber++;

            enemyNumberToShow += eNumber;
            levelUp = true;

            if (tempFirsteNumber != 0)
            {
                EnemyInstantiate();
            }
        }
    }

    public void Confirm()
    {
        if (enemyNumber.text != null)
        {
            eNumber = int.Parse(enemyNumber.text);
        }
        else
        {
            eNumber = 3;
        }

        enemyNumberToShow = eNumber;
        enemyNumText.text = eNumber.ToString();
        enemyNumberMenu.SetActive(false);
        singleStartMenu.SetActive(true);
    }

    public void singleAIStart()
    {
        menuSound.Stop();
        gameSound.Play();

        EnemyInstantiate();

        singleStartMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void EnemyInstantiate()
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

            enemy.GetComponent<AIDestinationSetter>().target = npos.transform;
        }

        globalEnemyNumber = eNumber;
    }

    public void Retry()
    {
        PlayerFind.playerFinded = false;
        PlayerBullet.playerShot = false;
        AIHealth.isEscape = false;
        //AIHealth.score = 0;
        AIDestinationSetter.wantSearch = false;

        GunPlayerFind.GunFindedplayer = false;
        PlayerBullet.playerShotToGun = false;

        SceneManager.LoadScene("single");
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
