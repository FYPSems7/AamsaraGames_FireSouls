using UnityEngine;
using System.Collections;

public class BulletCollisionScript : MonoBehaviour {

	public bool isDestoryed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("EnemyBody")) {
			isDestoryed = true;
			Destroy (other.transform.parent.gameObject);
		} else if (other.CompareTag ("Wall")) {
			isDestoryed = true;
		}else {
			isDestoryed = false;
		}
		if(isDestoryed)
			Destroy (gameObject , 0.05f);
	}
}
