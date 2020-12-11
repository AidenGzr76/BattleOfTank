using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	[HideInInspector]
	public GameObject playerFrom;

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
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
