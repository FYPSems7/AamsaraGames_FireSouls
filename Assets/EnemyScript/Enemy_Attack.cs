using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy_Attack : MonoBehaviour 
{
	public CharacterStatsScript characterstatsscript;

	private NavMeshAgent myNavMeshAgent;
	private Enemy_Master enemyMaster;
	private Transform attackTarget;
	private Transform myTransform;
	private float attackRate = 5.0f;
	private float nextAttack;
	public float attackRange = 3.5f;
	public int attackDamage = 10;
	private float Timer = 5.0f;
	private bool isJump = false;

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += DisableThis;
		enemyMaster.EventEnemySetNavTarget += SetAttackTarget;
	}

	void OnDisable()
	{
		enemyMaster.EventEnemyDie -= DisableThis;
		enemyMaster.EventEnemySetNavTarget -= SetAttackTarget;
	}

	void Start()
	{
		characterstatsscript = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}

	void Update () 
	{
		//Debug.Log (Timer);
		if (Timer > 0) 
		{
			Timer -= Time.deltaTime;
		}


		if (attackTarget != null && Timer <= 0.0f) 
		{
			myNavMeshAgent.speed = 100;
			myNavMeshAgent.acceleration = 1500;
			TryToAttack ();
		}

		/*
		if (isJump == true) 
		{
			myNavMeshAgent.speed = 3.5f;
			myNavMeshAgent.acceleration = 100f;
		}
		*/
	}

	void SetInitialReferences()
	{
		enemyMaster = GetComponent<Enemy_Master> ();
		myTransform = transform;

		if (GetComponent<NavMeshAgent> () != null) 
		{
			myNavMeshAgent = GetComponent<NavMeshAgent> ();
		}
	}

	void SetAttackTarget(Transform targetTransform)
	{
		attackTarget = targetTransform;
	}

	void TryToAttack()
	{
				if (Vector3.Distance (myTransform.position, attackTarget.position) <= attackRange) 
				{
			
					

					Vector3 lookAtVector = new Vector3 (attackTarget.position.x, myTransform.position.y, attackTarget.position.z);

					myTransform.LookAt (lookAtVector);
					//enemyMaster.CallEventEnemyAttack ();
					enemyMaster.CallEventEnemyJump ();
					
					enemyMaster.isOnRoute = false;

				}
			Timer = 5.0f;
			isJump = true;

			//myNavMeshAgent.speed = 3.5f;

	}

	public void OnEnemyAttack()
	{
		if (attackTarget != null) 
		{
			if(Vector3.Distance(myTransform.position, attackTarget.position) <= attackRange && 
				attackTarget.GetComponent<CharacterStatsScript>() != null)
				{
					Vector3 toOther = attackTarget.position - myTransform.position;
				Debug.Log (Vector3.Dot (toOther, myTransform.forward).ToString ());

					if(Vector3.Dot(toOther, myTransform.forward) > 0.5f)
					{
					characterstatsscript.PlayerHealth -= 10;
					characterstatsscript.movementSpeed *= 5;
					Debug.Log (characterstatsscript.movementSpeed);
					}
				}
		}
	}

	void DisableThis()
	{
		this.enabled = false;
	}
}
