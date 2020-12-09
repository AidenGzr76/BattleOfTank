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

	// Use this for initialization
	void Start()
	{		
		TankMovement pc = GetComponent<TankMovement>();
		isLocalPlayer = pc.isLocalPlayer;
	}

	public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;

		Network n = Network.instance.GetComponent<Network>();
		n.CommandHealthChange(playerFrom, this.gameObject, amount, isEnemy);
	}

	public void OnChangeHealth()
	{
		healthBar.value = currentHealth / maxHealth;

		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Explosion.SetBool("isExplore", true);

				if (isEnemy)
				{
					Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				}

				Invoke("stop", Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f) ;

				if (isLocalPlayer)
				{
					destroyControl = true;
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
