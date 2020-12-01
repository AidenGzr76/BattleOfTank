using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log(collision.transform.gameObject.name);

		if (collision.transform.tag == "Enemy")
		{
			PlayerFind.playerFinded = true;
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
