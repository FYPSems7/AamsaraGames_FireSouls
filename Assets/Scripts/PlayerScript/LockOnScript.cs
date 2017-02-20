using UnityEngine;
using System.Collections;

public class LockOnScript : MonoBehaviour {

	public Vector3 camTrans;
	public GameObject LockOnEnemy;

	// Use this for initialization
	void Start () {
		camTrans = Camera.main.transform.position;
		LockOnEnemy = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = LockOnEnemy.transform.position;
		transform.LookAt (camTrans);
	}
}
