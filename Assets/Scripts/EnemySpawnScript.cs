using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnScript : MonoBehaviour 
{
	public int enemyNumber = 30;
	public int SpawnedEnemyAmt = 0;
	public GameObject Player;
	public GameObject EnemyPrefab;
	public GameObject EnemyL;
	public GameObject SpawnObject;
	//public EnemyAttributeListScript enemyAttributeList;
	//public EnemyAttribute eA;
	float SpawnObjectWidth;
	float SpawnObjectHeight;
	[Range(1.0f, 3.0f)]
	public float ExtendedRangePercentage;
	public List<GameObject> EnemyList = new List<GameObject>();
	public float Area = 2.0f;
	public float DetectionDist = 20.0f;
	[Range(0.0f, 3.0f)]
	public float SpawnRate;
	public float SpawnTimer = 0.0f;
	bool up = false;
	bool down = false;
	bool left = false;
	bool right = false;

	public enum SpawnPattern
	{
		Area = 0,
		Surround,
		Infront
	}
	public SpawnPattern Spawn;
	public enum InfrontSpawnPattern
	{
		up = 0,
		down,
		left,
		right
	}
	public InfrontSpawnPattern infrntPattern;

	void Start()
	{
		GameObject SpawnPlatform = (GameObject)Instantiate (SpawnObject, transform.position, Quaternion.identity);
		//Spawn = EnemySpawnScript.SpawnPattern.Area;
		SpawnObjectWidth = SpawnObject.GetComponent<Renderer> ().bounds.size.x;
		infrontPattern ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (SpawnTimer > 0.0f) {
			SpawnTimer -= Time.fixedDeltaTime;
		}
		if (Vector3.Distance (transform.position, Player.transform.position) < DetectionDist) {
			if (SpawnedEnemyAmt < enemyNumber) {
				StartCoroutine ("Spawner");
			}
		} else
			StopCoroutine ("Spawner");

		foreach (GameObject n in EnemyList) {
			if (n == null) {
				EnemyList.Remove (n);
				SpawnedEnemyAmt--;
			}
		}
			
		/*foreach (EnemyAttribute n in enemyAttributeList.enemysattribute) {
			if(n.EnemyName == EnemyPrefab.name) {
				eA = n.Clone ();
			}
		}*/
	}

	IEnumerator Spawner()
	{
		switch (Spawn) {
		case SpawnPattern.Area:
			AreaSpawn ();
			break;
		case SpawnPattern.Surround:
			SurroundSpawn ();
			break;
		case SpawnPattern.Infront:
			SpawnInfront ();
			break;
		}
		yield return null;
	}

	void infrontPattern()
	{
		switch (infrntPattern) {
		case InfrontSpawnPattern.up:
			up = true;
			down = false;
			left = false;
			right = false;
			break;
		case InfrontSpawnPattern.down:
			up = false;
			down = true;
			left = false;
			right = false;
			break;
		case InfrontSpawnPattern.left:
			up = false;
			down = false;
			left = true;
			right = false;
			break;
		case InfrontSpawnPattern.right:
			up = false;
			down = false;
			left = false;
			right = true;
			break;
		}
	}

	void AreaSpawn()
	{
		if (SpawnTimer <= 0.0f) {
			float randomX = Random.Range (transform.position.x - Area, transform.position.x + Area);
			float randomZ = Random.Range (transform.position.z - Area, transform.position.z + Area);
			GameObject enemy = (GameObject)Instantiate (EnemyPrefab, new Vector3 (randomX, transform.position.y, randomZ), Quaternion.identity);
			//enemy.GetComponent<EnemyAttibute> ().eA = eA.Clone ();
			enemy.transform.SetParent (EnemyL.transform);
			EnemyList.Add (enemy);

			SpawnTimer = SpawnRate;
			SpawnedEnemyAmt++;
		}
	}

	void SurroundSpawn()
	{
		float distanceRange = SpawnObjectWidth * ExtendedRangePercentage;
		float randomX;
		float randomZ;
		if (SpawnTimer <= 0.0f) {
			randomX = Random.Range (transform.position.x - Area, transform.position.x + Area);
			randomZ = Random.Range (transform.position.z - Area, transform.position.x + Area);
			float distance = Mathf.Sqrt ((Mathf.Pow (transform.position.x - randomX, 2.0f)) + (Mathf.Pow (transform.position.z - randomZ, 2.0f)));
			//float distanceZ = Mathf.Sqrt ((Mathf.Pow (randomZ, 2.0f)) + (Mathf.Pow (transform.position.z, 2.0f)));
			if (distance > distanceRange && distance < Area) {
				GameObject enemy = (GameObject)Instantiate (EnemyPrefab, new Vector3 (randomX, transform.position.y, randomZ), Quaternion.identity);
				//enemy.GetComponent<EnemyAttibute> ().eA = eA.Clone ();
				enemy.transform.SetParent (EnemyL.transform);
				EnemyList.Add (enemy);
				SpawnTimer = SpawnRate;
				SpawnedEnemyAmt++;
			}
		}
	}

	void SpawnInfront()
	{
		float randomX;
		float randomZ; 
		float x = transform.position.x + (SpawnObjectWidth * ExtendedRangePercentage);
		float z = transform.position.z - (SpawnObjectWidth * ExtendedRangePercentage);
		//Making all Spawn front as view is below
		if (SpawnTimer <= 0.0f) {
			randomX = randX();
			randomZ = randZ();
			GameObject enemy = (GameObject)Instantiate (EnemyPrefab, new Vector3 (randomX, transform.position.y, randomZ), Quaternion.identity);
			//enemy.GetComponent<EnemyAttibute> ().eA = eA.Clone ();
			enemy.transform.SetParent (EnemyL.transform);
			EnemyList.Add (enemy);
			SpawnTimer = SpawnRate;
			SpawnedEnemyAmt++;
		}
	}

	float randX()
	{
		float x = 0.0f;
		float x1 = 0.0f;
		if (up) {
			x = transform.position.x + (SpawnObjectWidth * ExtendedRangePercentage);
			x1 = Random.Range (x - Area, x + Area);
		} else if (down) {
			x = transform.position.x + (SpawnObjectWidth * ExtendedRangePercentage);
			x1 = Random.Range (x - Area, x + Area);
		} else if (left) {
			x = transform.position.x - (SpawnObjectWidth * ExtendedRangePercentage);
			x1 = Random.Range (x, x - Area);
		} else if (right) {
			x = transform.position.x + (SpawnObjectWidth * ExtendedRangePercentage);
			x1 = Random.Range (x, x + Area);
		}
		return x1;
	}

	float randZ()
	{
		float z = 0.0f;
		float z1 = 0.0f;
		if (up) {
			z = transform.position.z + (SpawnObjectWidth * ExtendedRangePercentage);
			z1 =Random.Range (z, z + Area);
		} else if (down) {
			z = transform.position.z - (SpawnObjectWidth * ExtendedRangePercentage);
			z1= Random.Range (z, z - Area);
		} else if (left) {
			z = transform.position.z + (SpawnObjectWidth * ExtendedRangePercentage);
			z1 = Random.Range (z - Area, z + Area);
		} else if (right) {
			z = transform.position.z + (SpawnObjectWidth * ExtendedRangePercentage);
			z1 = Random.Range (z - Area, z + Area);
		}
		return z1;
	}
}
