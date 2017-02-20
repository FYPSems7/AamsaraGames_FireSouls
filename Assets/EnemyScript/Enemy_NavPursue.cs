using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy_NavPursue : MonoBehaviour 
{
	private Transform myTarget;
	private Transform myTransform;
	private Enemy_Master enemyMaster;
	private NavMeshAgent myNavMeshAgent;
	private float checkRate;
	private float nextCheck;
	public float detectionRange;

	private bool isUp = true;
	private bool isJump = true;
	private float jumpSpeed = 1.2f;
	private float jumpHeight = 10.0f;
	private float jumpDistance = 10.0f;

	void Awake()
	{
		myTransform = transform;
	}

	void Start()
	{
		myTarget = GameObject.FindWithTag ("Player").transform;
	}

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += DisableThis;
	}

	void OnDisable()
	{
		enemyMaster.EventEnemyDie -= DisableThis;
	}
		

	void Update()
	{  
		detectionRange = 10f;

		if (Time.time > nextCheck) 
		{

			nextCheck = Time.time + checkRate;

			TryToChaseTarget ();



		}
	}

	void SetInitialReferences()
	{
		enemyMaster = GetComponent<Enemy_Master> ();
		if (GetComponent<NavMeshAgent> () != null) 
		{
			myNavMeshAgent = GetComponent<NavMeshAgent> ();
		}
		checkRate = Random.Range (0.1f, 0.2f);
	}

	void TryToChaseTarget()
	{

		isUp = true;
		isJump = true; 

		if (enemyMaster.myTarget != null && myNavMeshAgent != null && !enemyMaster.isNavPaused) 
		{
			myNavMeshAgent.SetDestination (enemyMaster.myTarget.position);

			if (myNavMeshAgent.remainingDistance < jumpDistance) 
			{
				//myNavMeshAgent.Stop ();
				//TryToJump ();
				
				if (myNavMeshAgent.remainingDistance > myNavMeshAgent.stoppingDistance) 
				{
					enemyMaster.CallEventEnemyWalking ();
					enemyMaster.isOnRoute = true;
				}
			}

		}

	}

	void TryToJump()
	{
		Vector3 pos = transform.position;

		Debug.Log ("Orginal: " + pos.y);

		if (Vector3.Distance (myTransform.position, myTarget.position) <= jumpDistance) 

		{

			if (isUp && isJump) 
			{

				jumpSpeed *= 0.8f;
				Debug.Log ("New :" + pos.y);
				//pos.y += jumpSpeed ;
				pos.y += jumpSpeed;
				//transform.position = pos;
				if (pos.y >= jumpHeight) 
				{
					isUp = false;
				} 
			}
			else if (!isUp && isJump) 
			{
				Debug.Log ("Run");
				pos.y -= jumpSpeed;
				jumpSpeed *= 1.3f;


				if (pos.y <= 0.0f) 
				{
					pos.y = 0.0f;
					jumpSpeed = 1.5f;
					isJump = false;
					isUp = true;
				}
			}

			transform.position = pos;
		}
	}

	void DisableThis()
	{
		if (myNavMeshAgent != null) 
		{
			myNavMeshAgent.enabled = false;

		}

		this.enabled = false;
	}

}
