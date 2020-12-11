using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	public static bool playerShot = false;
	public static string playerShotName;

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log(collision.transform.gameObject.name);

		if (collision.transform.tag == "Enemy")
		{
			playerShot = true;
			playerShotName = collision.transform.name;
			//AI._target = 
			var hit = collision.gameObject;
			var health = hit.GetComponent<AIHealth>();
			if (health != null)
			{
				health.TakeDamage(collision.gameObject, 10);
			}

			Destroy(gameObject);
		}
	}
}
