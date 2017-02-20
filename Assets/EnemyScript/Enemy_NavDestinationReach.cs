using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy_NavDestinationReach : MonoBehaviour 
{

	private Enemy_Master enemyMaster;
	private NavMeshAgent myNavMeshAgent;
	private float checkRate;
	private float nextCheck;

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += DisableThis;
	}

	void OnDisable()
	{

		enemyMaster.EventEnemyDie -= DisableThis;
	}


	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > nextCheck) 
		{
			nextCheck = Time.time + checkRate;
			CheckDestinationReached ();
		}
	
	}


	void SetInitialReferences()
	{
		enemyMaster = GetComponent<Enemy_Master> ();
		if (GetComponent<NavMeshAgent> () != null) 
		{
			myNavMeshAgent = GetComponent<NavMeshAgent> ();
		}
		checkRate = Random.Range (0.3f, 0.6f);
	}

	void CheckDestinationReached()
	{
		if(enemyMaster.isOnRoute)
			{
				if(myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
				{
					enemyMaster.isOnRoute = false;
					enemyMaster.CallEventEnemyReachedNavTarget();
				}
			}

	}

	void DisableThis()
	{

		this.enabled = false;
	}
}
