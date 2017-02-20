using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kukkuta : MonoBehaviour {


	public CharacterStatsScript characterstatsscript;
	public float initialAngle;

	private Transform myTarget;
	private Transform myTransform;
	private float attackRange = 9.0f;
	private float detectionRange = 20.0f;
	private float moveSpeed = 5.0f;
	private Rigidbody rb;
	private bool finishJump;
	private float Timer;
	private float Timer2;
	private float attackRate;
	private float attackDamage = 5.0f;
	private bool isReduced = false;



	void Awake()
	{
		myTransform = transform;
	}

	void Start ()
	{
		finishJump = false;

		rb = GetComponent<Rigidbody> ();

		if (myTarget == null) 
		{
			myTarget = GameObject.FindWithTag ("Player").transform;
		}

		characterstatsscript = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();

	}


	void Update()
	{

		transform.LookAt(myTarget);

		if (Vector3.Distance (transform.position, myTarget.position) <= detectionRange) 
		{

			transform.position += transform.forward * moveSpeed * Time.deltaTime;


			if(Vector3.Distance(transform.position,myTarget.position) <= attackRange)
			{
				Jump ();

				if (finishJump == true) 
				{
					transform.parent = myTarget.transform;
					Timer += Time.deltaTime;
					if (!isReduced) {
						characterstatsscript.movementSpeed -= 1.0f;
						isReduced = true;
					}
					if (Timer >= 0.5) 
					{
						Timer2 += Time.deltaTime;
						moveSpeed = 0.0f;

						if (Timer2 >= 0.5) 
						{
							rb.isKinematic = true;
							BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
							collider.isTrigger = true;
							Attack ();

						}

					}


				}
			} 
		}


			
			

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag("PlayerAttack"))
			
			Destroy(gameObject);

	}

	void Attack()
	{
		
		attackRate -= Time.deltaTime;

		if (attackRate <= 0) 
		{
			//characterstatsscript.PlayerHealth -= attackDamage;
			characterstatsscript.DmgTaken (attackDamage);
			Debug.Log (moveSpeed);
			attackRate = 1.1f;
		}
	}



	void Jump () 
	{

		Vector3 playerPos = myTarget.position;

		float gravity = Physics.gravity.magnitude;
		// Selected angle in radians
		float angle = initialAngle * Mathf.Deg2Rad;

		// Positions of this object and the target on the same plane
		Vector3 planarTarget = new Vector3(playerPos.x, 0, playerPos.z);
		Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

		// Planar distance between objects
		float distance = Vector3.Distance(planarTarget, planarPostion);


		// Distance along the y axis between objects
		float yOffset = transform.position.y - playerPos.y;

		float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

		Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

		// Rotate our velocity to match the direction between the two objects
		float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion); //* (playerPos.x > transform.position.x ? 1 : -1);
		Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

		// Fire!
		rb.velocity = finalVelocity;

		finishJump = true;

	}
		
}
