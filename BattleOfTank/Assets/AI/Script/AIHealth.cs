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

	public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;

		OnChangeHealth();
	}

	public void OnChangeHealth()
	{
		healthBar.value = currentHealth / maxHealth;

		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			}
			else
			{
				currentHealth = maxHealth;
				healthBar.value = currentHealth / maxHealth;
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
