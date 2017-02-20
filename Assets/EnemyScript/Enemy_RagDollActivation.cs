using UnityEngine;
using System.Collections;

public class Enemy_RagDollActivation : MonoBehaviour 
{
	private Enemy_Master enemyMaster;
	private Collider myCollider;
	private Rigidbody myRigidbody;

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += ActivateRagDoll;
	}

	void OnDisable()
	{
		enemyMaster.EventEnemyDie -= ActivateRagDoll;
	}

	void SetInitialReferences()
	{
		enemyMaster = transform.root.GetComponent<Enemy_Master> ();

		if (GetComponent <Collider> () != null) 
		{
			myCollider = GetComponent <Collider> ();
		}

		if (GetComponent <Rigidbody> () != null) 
		{
			myRigidbody = GetComponent <Rigidbody>();
		}

	}

	void ActivateRagDoll()
	{
		if (myRigidbody != null) 
		{
			myRigidbody.isKinematic = false;
			myRigidbody.useGravity = true;
		}

		if (myCollider != null) 
		{
			myCollider.isTrigger = false;
			myCollider.enabled = true;
		}
	}

}
