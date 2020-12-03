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
				//Explosion.gameObject.transform.position = gameObject.transform.position;
				Explosion.SetBool("isExplore", true);

				Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
				//Destroy(gameObject);
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
