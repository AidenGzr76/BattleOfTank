using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public const int maxHealth = 100;
	public bool destroyOnDeath;

	public int currentHealth = maxHealth;
	public bool isEnemy = false;

	private bool isLocalPlayer;

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
		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			}
			else
			{
				currentHealth = maxHealth;
				Respawn();
			}
		}
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
