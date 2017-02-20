using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy_NavPause : MonoBehaviour 
{
	private Enemy_Master enemyMaster;
	private NavMeshAgent myNavMeshAgent;
	private float pauseTIme = 1;

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += DisableThis;
		enemyMaster.EventEnemyDeductHealth += PauseNavMeshAgent;
	}

	void OnDisable()
	{
		enemyMaster.EventEnemyDie -= DisableThis;
		enemyMaster.EventEnemyDeductHealth -= PauseNavMeshAgent;
	}

	void SetInitialReferences()
	{
		enemyMaster = GetComponent<Enemy_Master> ();
		if (GetComponent<NavMeshAgent> () != null) 
		{
			myNavMeshAgent = GetComponent<NavMeshAgent> ();
		}

	}

	void PauseNavMeshAgent(int dummy)
	{
		if (myNavMeshAgent != null) 
		{
			if(myNavMeshAgent.enabled)
				{
					myNavMeshAgent.ResetPath();
					enemyMaster.isNavPaused = true;
					StartCoroutine(RestartNavMeshAgent());

				}
		}

	}

	IEnumerator RestartNavMeshAgent()
	{
		yield return new WaitForSeconds (pauseTIme);
		enemyMaster.isNavPaused = false;
	}

	void DisableThis()
	{
		StopAllCoroutines ();
	}
}
