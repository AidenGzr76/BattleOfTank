using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public const float maxHealth = 100;
	public bool destroyOnDeath;

	public float currentHealth = maxHealth;
	public bool isEnemy = false;

	private bool isLocalPlayer;
	
	public Slider healthBar;

	public Animator Explosion;

	public static bool destroyControl = false;
	public static bool respawnControl = false;

	public static int score = 0;

	private AudioSource explosionSound;

	bool enemyDeath = false;

	// Use this for initialization
	void Start()
	{		
		TankMovement pc = GetComponent<TankMovement>();
		isLocalPlayer = pc.isLocalPlayer;

		explosionSound = GameObject.Find("explosionSound").GetComponent<AudioSource>();
	}

	public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;

        if (currentHealth <= 0)
        {
			enemyDeath = true;
        }

		Network n = Network.instance.GetComponent<Network>();
		n.CommandHealthChange(playerFrom, this.gameObject, amount, isEnemy);
	}

	public void OnChangeHealth()
	{
		healthBar.value = currentHealth / maxHealth;

		if (enemyDeath)
		{
			enemyDeath = false;

			if (destroyOnDeath)
			{
				Explosion.SetBool("isExplore", true);

				explosionSound.Play();

				if (isEnemy)
				{
					Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				}

				Invoke("stop", Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f) ;

				if (isLocalPlayer)
				{
                    destroyControl = true;
                }
                else
                {
					score++;
                }
			}
		}
	}

	private void stop()
	{
		Explosion.SetBool("isExplore", false);
		Respawn();
	}

	public void menuBtn()
	{
		respawnControl = true;
	}

	void Respawn()
	{
		if (isLocalPlayer)
		{
			currentHealth = maxHealth;
			healthBar.value = currentHealth / maxHealth;
			Vector2 spawnPoint = Vector2.zero;
			transform.position = spawnPoint;
			Explosion.SetBool("isExplore", false);
		}
	}
}
