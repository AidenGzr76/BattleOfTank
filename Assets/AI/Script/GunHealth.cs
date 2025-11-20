using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHealth : MonoBehaviour
{
	public const float maxHealth = 100;

	public float currentHealth = maxHealth;

	public Slider healthBar;

	public Animator Explosion;

	public static int score = 0;

	private AudioSource explosionSound;

	public static bool isDestroy = false;
	public static string destroyedName;


	private void Start()
	{
		explosionSound = GameObject.Find("explosionSound").GetComponent<AudioSource>();
	}

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
			Explosion.SetBool("isExplore", true);

			explosionSound.Play();

			GunAI.allowFire = false;
			GunPlayerFind.GunFindedplayer = false;

			isDestroy = true;
			//destroyedName = transform.name;

            Destroy(gameObject, Explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.1f);
        }
        else
        {
			isDestroy = false;
        }
	}
}
