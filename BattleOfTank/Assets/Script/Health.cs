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

				Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				//Destroy(gameObject);
				//Respawn();
				//StartCoroutine("temp");
			}
			else
			{
				currentHealth = maxHealth;
				healthBar.value = currentHealth / maxHealth;
				//Respawn();
			}
		}
	}

	IEnumerator temp()
	{
		yield return new WaitForSeconds(Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		Explosion.SetBool("isExplore", false);
		//healthBar.value = currentHealth / maxHealth;
		Respawn();
	}

	void Respawn()
	{
		if (isLocalPlayer)
		{
			Vector2 spawnPoint = Vector2.zero;
			Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);
			transform.position = spawnPoint;
			transform.rotation = spawnRotation;
		}
	}
}
