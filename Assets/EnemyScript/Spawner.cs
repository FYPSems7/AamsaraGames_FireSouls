using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public int numberOfEnemy;
	public float spawnRadius;
	private Vector3 spawnPosition;


	// Use this for initialization
	void Start () 
	{
		SpawnEnemy();
	}
	
	void SpawnEnemy()
	{
		for (int i = 0; i < numberOfEnemy; i++) 
		{
			spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
		//	Instantiate (enemyPrefab, spawnPosition, Quaternion.identity);
			Instantiate (enemyPrefab, new Vector3 (Random.Range (-50, 50), 0.0f, Random.Range (-50, 50)), Quaternion.identity);
		}
	}
}
