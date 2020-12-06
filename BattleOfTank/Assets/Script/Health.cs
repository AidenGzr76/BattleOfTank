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

		//OnChangeHealth();
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

				//Invoke("Respawn", Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);

				if (isEnemy)
				{
					Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				}

				Invoke("stop", Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f) ;

				//Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				//currentHealth = maxHealth;
				//healthBar.value = currentHealth / maxHealth;
				//Respawn();
				if (isLocalPlayer)
				{
					destroyControl = true;
				}
			}
			//else
			//{
			//	Debug.Log("Ddsdsd");
			//	currentHealth = maxHealth;
			//	healthBar.value = currentHealth / maxHealth;
			//	Respawn();
			//}
		}
	}

	private void stop()
	{
		Explosion.SetBool("isExplore", false);
		//Explosion.gameObject.SetActive(false);
		Respawn();
	}

	public void menuBtn()
	{
		//Respawn();
		respawnControl = true;
		Debug.Log("eeee");
	}

	void Respawn()
	{
		if (isLocalPlayer)
		{
			currentHealth = maxHealth;
			healthBar.value = currentHealth / maxHealth;
			//respawnControl = true;
			//Debug.Log(":))))");
			Vector2 spawnPoint = Vector2.zero;
			//Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);
			transform.position = spawnPoint;
			//transform.rotation = spawnRotation;
			//Explosion.gameObject.SetActive(true);
			Explosion.SetBool("isExplore", false);
		}
	}
}
