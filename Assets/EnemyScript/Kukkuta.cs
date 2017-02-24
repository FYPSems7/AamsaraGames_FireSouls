using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kukkuta : MonoBehaviour {


	public CharacterStatsScript characterstatsscript;

	public float initialAngle;
	private GameObject target;
	private Transform myTarget;
	private Transform myTransform;
	private float attackRange = 10.0f;
	private float detectionRange = 30.0f;
	private float moveSpeed = 5.0f;
	private Rigidbody rb;
	private float Timer;
	private float Timer2;
	private float attackRate;
	private float attackDamage = 5.0f;
	private bool isReduced = false;
	public bool isDetected= false;
	private bool isRun = true;
	private bool isjump;
	public float dirNum;

	void Awake()
	{
		myTransform = transform;
		target = GameObject.FindGameObjectWithTag ("Player");
		characterstatsscript = target.GetComponent <CharacterStatsScript> ();
	}

	void Start ()
	{
		isReduced = false;
		isjump = false; 

		rb = GetComponent<Rigidbody> ();

		if (myTarget == null) 
		{
			myTarget = GameObject.FindWithTag ("Player").transform;
		}

	}
		

	void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("PlayerAttack")) 
		{
			Destroy (gameObject);
		}

		if (other.gameObject == target) 
		{
			transform.parent = myTarget.transform;
			moveSpeed = 0.0f;
			rb.isKinematic = true;
			BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
			collider.isTrigger = true;
			Attack();

		} 

		else if (isjump)
			
		{
			moveSpeed = 0;
		}

	}


	void Update()
	{
		Vector3 heading = myTarget.position - transform.position;

		if (myTransform.position.y < 0) 
		{
			isRun = false; 
			myTransform.position = new Vector3 (myTransform.position.x, 0.0f, myTransform.position.z);
		} 
		else 
		{
			isRun = true;
		}
			
		if (isjump == true && isDetected == true && transform.parent != myTarget) 
		{
			
			Timer += Time.deltaTime;

			if (Timer >= 3) 
			{
				isjump = false;
				isDetected = false;
				Timer = 0;
				moveSpeed = 5f;
			}
		}
	}

	void FixedUpdate()
	{
		


		if (Vector3.Distance (transform.position, myTarget.position) <= detectionRange) 
		{
			transform.LookAt(myTarget, transform.up);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

			transform.position += transform.forward * moveSpeed * Time.deltaTime;


			if(Vector3.Distance(transform.position,myTarget.position) <= attackRange)
			{
				Jump ();
			} 
		}
	}



	void Attack()
	{
		
		attackRate -= Time.deltaTime;

		if (attackRate <= 0) 
		{
			characterstatsscript.PlayerHealth -= attackDamage;
			characterstatsscript.DmgTaken (attackDamage);
			//Debug.Log (moveSpeed);
			attackRate = 1.1f;

			if (!isReduced) 
			{
				characterstatsscript.movementSpeed -= 1.0f;
				isReduced = true;
			}
		}
	}

	void Jump () 
	{
		RaycastHit hit;

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position, fwd * 100,Color.cyan);

		if(Physics.Raycast(transform.position,fwd, out hit, 100) && !isDetected  && !isjump)

		{
			
			Debug.Log (hit.transform.name);

			if (hit.collider.gameObject.tag == "Player") 
			{
				//isDetected = true;

				//target = hit.collider.gameObject;

				target = hit.transform.gameObject;


				float gravity = Physics.gravity.magnitude;
				// Selected angle in radians
				float angle = initialAngle * Mathf.Deg2Rad;

				// Positions of this object and the target on the same plane
				Vector3 targetPosition = new Vector3(target.transform.position.x, 0,target.transform.position.z);
				Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

				// Planar distance between objects
				float distance = Vector3.Distance(targetPosition, currentPosition);

				// Distance along the y axis between objects
				float yOffset = transform.position.y -target.transform.position.y;

				float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

				Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

				// Rotate our velocity to match the direction between the two objects
				float angleBetweenObjects = Vector3.Angle(Vector3.forward, target.transform.position - currentPosition)* (target.transform.position.x > transform.position.x ? 1 : -1);; 


				Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
				//Debug.Log ("Distance : " + distance);
				// Fire!
				rb.velocity = finalVelocity ;
				//Debug.Log ("Final Velocity : " + finalVelocity);
				isjump = true;
				isDetected = true;
			}


		}

	}
		
}
