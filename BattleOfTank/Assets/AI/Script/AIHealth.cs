using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
	public const int maxHealth = 100;
	public bool destroyOnDeath;

	public int currentHealth = maxHealth;
	public bool isEnemy = false;

	private bool isLocalPlayer;

	public static bool isEscape;

	public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;

		OnChangeHealth();
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
			}
		}
		if (isEnemy)
		{
			if (currentHealth <= 40)
			{
				isEscape = true;
			}
		}
	}
}
