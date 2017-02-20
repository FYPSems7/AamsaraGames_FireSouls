using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

	public GameObject player;
	float range = 10f;
	public float bulletImpulse = 1.0f;
	private bool onRange= false;
	public Rigidbody Bullet;
	private RaycastHit Hit;

	void Start()
	{
		float rand = Random.Range (1.5f, 2.0f);		//Shoot Delay
		InvokeRepeating("Shoot", 2, rand);
		player = GameObject.Find ("Player");
	}



	void Shoot()
	{

		if (onRange)
		{
			Rigidbody bullet = (Rigidbody)Instantiate(Bullet, transform.position + transform.forward, transform.rotation);
			var desiredDirection = (player.transform.position - transform.position);
			bullet.AddForce(desiredDirection * bulletImpulse, ForceMode.Impulse);
			Destroy (bullet.gameObject, 2);
		}
				

	}



	void Update() 
	{

		onRange = Vector3.Distance(transform.position, player.transform.position) < range;

	}


}