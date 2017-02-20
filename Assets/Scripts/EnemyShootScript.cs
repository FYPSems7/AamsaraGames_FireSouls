using UnityEngine;
using System.Collections;

public class EnemyShootScript : MonoBehaviour {
	
	public GameObject Bullet_Emitter;
	public GameObject Bullet;
	public float Bullet_Forward_Force = 200.0f;
	public bool Shoot = false;

	public float shootTimer = 0.0f;
	public float shootDuration = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (Shoot) {
			shootTimer += Time.deltaTime;
			if(shootTimer>= shootDuration)
			{
				shootTimer = 0.0f;
				GameObject Temporary_Bullet_Handler;
				Temporary_Bullet_Handler = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

				Rigidbody Temporary_RigidBody;
				Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody> ();

				Temporary_RigidBody.AddForce (transform.forward * Bullet_Forward_Force);


				Destroy (Temporary_Bullet_Handler, 10.0f);
			}
		}
	}
}
