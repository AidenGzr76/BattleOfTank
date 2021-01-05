using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

	public GameObject enemy;
	public GameObject spawnPoint;
	public int numberOfEnemies;
	[HideInInspector]
	public List<SpawnPoint> enemySpawnPoints;

	// Use this for initialization
	void Start()
	{
		// set the random spawn points over here
		//for (int i = 0; i < numberOfEnemies; i++)
		//{
		//	var spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-8f, 8f), 0f);
		//	var spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 180));
		//	SpawnPoint enemySpawnPoint = (Instantiate(spawnPoint,
		//											  spawnPosition,
		//											  spawnRotation)
		//								  as GameObject).GetComponent<SpawnPoint>();
		//	enemySpawnPoints.Add(enemySpawnPoint);
		//}
		//SpawnEnemies();
	}

	public void SpawnEnemies(Network.EnemiesJSON enemiesJSON)
	{
		foreach (Network.UserJSON enemyJSON in enemiesJSON.enemies)
		{
			if (enemyJSON.health <= 0)
			{
				continue;
			}
			Vector3 position = new Vector3(float.Parse(enemyJSON.position[0]), float.Parse(enemyJSON.position[1]), float.Parse(enemyJSON.position[2]));
			Quaternion rotation = Quaternion.Euler(float.Parse(enemyJSON.rotation[0]), float.Parse(enemyJSON.rotation[1]), float.Parse(enemyJSON.rotation[2]));
			GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
			newEnemy.name = enemyJSON.name;
			TankMovement pc = newEnemy.GetComponent<TankMovement>();
			pc.isLocalPlayer = false;
			Health h = newEnemy.GetComponent<Health>();
			h.currentHealth = enemyJSON.health;
			h.OnChangeHealth();
			h.destroyOnDeath = true;
			h.isEnemy = true;
		}
	}
}
