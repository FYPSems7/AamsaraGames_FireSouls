using UnityEngine;
using System.Collections;

public class Enemy_Animation : MonoBehaviour 
{
	private Enemy_Master enemyMaster;
	private Animator myAnimator;

	void OnEnable()
	{
		SetInitialReferences ();
		enemyMaster.EventEnemyDie += DisableAnimation;
		enemyMaster.EventEnemyWalking += SetAnimationToWalk;
		enemyMaster.EventEnemyReachedNavTarget += SetAnimationToToldle;
		enemyMaster.EventEnemyAttack += SetAnimationToAttack;
		enemyMaster.EventEnemyDeductHealth += SetAnimationToStruck;
		enemyMaster.EventEnemyJump += SetAnimationToJump;
	}

	void OnDisable()
	{
		enemyMaster.EventEnemyDie -= DisableAnimation;
		enemyMaster.EventEnemyWalking -= SetAnimationToWalk;
		enemyMaster.EventEnemyReachedNavTarget -= SetAnimationToToldle;
		enemyMaster.EventEnemyAttack -= SetAnimationToAttack;
		enemyMaster.EventEnemyDeductHealth -= SetAnimationToStruck;
		enemyMaster.EventEnemyJump -= SetAnimationToJump;
	}

	void SetInitialReferences()
	{
		enemyMaster = GetComponent <Enemy_Master>();

		if (GetComponent<Animator>() != null) 
		{
			myAnimator = GetComponent<Animator>();
		}
	}

	void SetAnimationToWalk()
	{
		if (myAnimator != null) 
		{
			if (myAnimator.enabled) 
			{
				myAnimator.SetBool ("isPursuing", true);
				myAnimator.SetBool ("isJumping", false);
			}
		
		}
	}

	void SetAnimationToJump()
	{
		if (myAnimator != null)
		{
			if (myAnimator.enabled) 
			{
				myAnimator.SetBool ("isJumping", true);
				//myAnimator.SetBool ("isPursuing", false);
			}
		}
	}

	void SetAnimationToToldle()
	{
		if (myAnimator != null) 
		{
			if (myAnimator.enabled) 
			{
				myAnimator.SetBool ("isPursuing", false);
			}

		}
	}

	void SetAnimationToAttack()
	{
		if (myAnimator != null) 
		{
			if (myAnimator.enabled) 
			{
				myAnimator.SetTrigger ("Attack");
			}

		}
	}

	void SetAnimationToStruck(int dummy)
	{
		if (myAnimator != null) 
		{
			if (myAnimator.enabled) 
			{
				myAnimator.SetTrigger ("Struck");
			}

		}
	}

	void DisableAnimation()
	{
		if (myAnimator != null) 
		{
			myAnimator.enabled = false;
		}
	}
}
