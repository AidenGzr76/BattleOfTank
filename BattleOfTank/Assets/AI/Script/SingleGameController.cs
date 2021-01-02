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
    public Text playerScore;

    private int currentEnemyNumber;
    private int allEnemyNumber;

    public GameObject MainCamera;

    public Text Kills;

    private AudioSource menuSound;
    private AudioSource gameSound;

    private int wave = 1;
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

        waveText.text = wave.ToString();
    }

    int showEnemyNum = 1;

    public void FixedUpdate()
    {
        Kills.text = AIHealth.score + "/" + showEnemyNum;
        //waveText.text = wave.ToString();

        if (AIHealth.destroyPlayer)
        {
            MainCamera.SetActive(true);

            singleEndMenu.SetActive(true);
            playerScore.text = AIHealth.score.ToString();
            AIHealth.score = 0;
            Time.timeScale = 0;

            AIHealth.destroyPlayer = false;
        }
        if (AIHealth.score == showEnemyNum)
        {
            PlayerFind.playerFinded = false;
            PlayerBullet.playerShot = false;
            AIHealth.isEscape = false;
            AIDestinationSetter.wantSearch = false;

            GunPlayerFind.GunFindedplayer = false;
            PlayerBullet.playerShotToGun = false;
            currentEnemyNumber += 1;
            showEnemyNum += currentEnemyNumber;

            EnemyInstantiate(currentEnemyNumber);

            wave++;

            waveText.text = wave.ToString();
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
            eNumber = 5;
        }

        showEnemyNum = eNumber;
        currentEnemyNumber = eNumber;
        enemyNumText.text = eNumber.ToString();
        enemyNumberMenu.SetActive(false);
        singleStartMenu.SetActive(true);
    }

    public void singleAIStart()
    {
        menuSound.Stop();
        gameSound.Play();

        EnemyInstantiate(eNumber);

        singleStartMenu.SetActive(false);
        Time.timeScale = 1;
    }

    int enemynumber = 0;

    void EnemyInstantiate(int numberOfEnemy)
    {

        for (int i = 0; i < numberOfEnemy; i++)
        {
            GameObject enemy = Instantiate(
                Enemy,
                enemySpawnPos[Random.Range(0, enemySpawnPos.Length)].transform.position,
                Quaternion.Euler(0, 0, Random.Range(0, 180))
                );

            enemynumber += 1;

            enemy.name = "Enemy" + "-" + enemynumber.ToString();

            GameObject npos = Instantiate(
                nextpos,
                new Vector3(Random.Range(-24, 24), Random.Range(-18, 10), 0),
                Quaternion.Euler(0, 0, Random.Range(0, 180))
                );

            npos.name = "nextPos" + "-" + enemynumber.ToString();

            enemy.GetComponent<AIDestinationSetter>().target = npos.transform;
        }

        //currentEnemyNumber = numberOfEnemy;
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
