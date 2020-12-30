using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
	public const float maxHealth = 100;
	public bool destroyOnDeath;

	public float currentHealth = maxHealth;
	public bool isEnemy = false;

	private bool isLocalPlayer;

	public static bool isEscape;

	public Slider healthBar;

	public Animator Explosion;

	public static int score = 0;

	public static bool destroyPlayer = false;

	public static string healthEnemyName;

	string tempName = null;

	private AudioSource explosionSound;

	bool isDeath = false;

	private void Start()
    {
		explosionSound = GameObject.Find("explosionSound").GetComponent<AudioSource>();
	}

    public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;

		if (currentHealth <= 0)
		{
			isDeath = true;
		}

		OnChangeHealth();
	}

	public void OnChangeHealth()
	{
		healthBar.value = currentHealth / maxHealth;

		if (isDeath)
		{
			isDeath = false;

			if (destroyOnDeath)
			{
				//Explosion.gameObject.transform.position = gameObject.transform.position;
				Explosion.SetBool("isExplore", true);

				explosionSound.Play();

				Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				//Destroy(gameObject);
			}
			else
			{
				currentHealth = maxHealth;
				healthBar.value = currentHealth / maxHealth;
			}

            if (isEnemy)
            {
                if (tempName != transform.name)
                {
					score++;
					tempName = transform.name;
				}
            }
            else
            {
				destroyPlayer = true;
			}
		}
		if (isEnemy)
		{
			if (currentHealth <= 40)
			{
				healthEnemyName = transform.name;
				isEscape = true;
			}
		}
	}
}
