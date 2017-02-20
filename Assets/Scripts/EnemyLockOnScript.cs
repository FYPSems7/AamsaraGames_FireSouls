using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLockOnScript : MonoBehaviour 
{
	public GameObject enemy;
	public GameObject EnemyLockedOn;
	public GameObject LockOnFrame;
	public List<GameObject>enemyLocation;
	public GameObject go = null;
	public bool isEnemyLocked = false;
	public static bool isLockOn = false;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (EnemyLockedOn == null && PlayerInScreen() == true) {
				if (FindClosestEnemy () != null) {
					isLockOn = true;
					EnemyLockedOn = FindClosestEnemy ();
					go = (GameObject)Instantiate (LockOnFrame, EnemyLockedOn.transform.position, Quaternion.identity);
					go.transform.parent = EnemyLockedOn.transform;
				} else
					isLockOn = false;
			} 
		}
		else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			isLockOn = false;
			Destroy (go);
			EnemyLockedOn = null;
		}
	}

	void LateUpdate()
	{
		if (EnemyLockedOn == null) {
			isLockOn = false;
		}
	}

	GameObject FindClosestEnemy()
	{
		GameObject closest = null;
		enemyLocation = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		float dist = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject n in enemyLocation) {
			Vector3 diff = (n.transform.position - position);
			float curDistance = diff.sqrMagnitude;
			if (curDistance < dist) {
				closest = n;
				dist = curDistance;
			}
		}
		enemyLocation.Clear ();
		return closest;
	}

	bool PlayerInScreen()
	{
		float screenx = Camera.main.pixelWidth - enemy.GetComponent<Renderer>().bounds.size.x;
		float screeny = Camera.main.pixelWidth - enemy.GetComponent<Renderer>().bounds.size.y;
		GameObject go = FindClosestEnemy ();
		Vector3 goScreenPos = Camera.main.WorldToScreenPoint (go.transform.position);

		if (goScreenPos.x <= screenx && goScreenPos.y <= screeny && goScreenPos.x >0 && goScreenPos.y > 0) {
			return true;
		} else 
			return false;
	}
}
