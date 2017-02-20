using UnityEngine;
using System.Collections;

public class PlayerAttackScript : MonoBehaviour {

	public EnemyLockOnScript enemyLockOnScript;
	public PlayerMovementScript playermovementScript;
	public CharacterStatsScript characterstats;
	public GameObject Melee_Emitter;
	public GameObject Melee;
	public GameObject Melee2;
	public GameObject SpinAttack;
	public GameObject ChargeBarUI;

	public GameObject Bullet_Emitter;
	public GameObject Bullet;
	GameObject temp_SpinAtt;
	GameObject temp_bullet;
	public float Bullet_Forward_Force = 200.0f;
	public float MeleeAttRate = 0.0f;
	public float RangeAttRate = 0.0f;
	public static int numberofattack = 1;
	public float ChargeTime = 3.0f;
	public float ChargeDuration = 0.0f;
	public float SpinDuration = 0.0f;
	float angle = 0.0f;
	public static bool spinAttack = false;
	GameObject MeleeEnemy;

	public float AttackComboCancelTimer = 0.0f;
	float ACCDuration = 1.5f;

	bool Alive = true;
	public bool isAttack = false;

	public enum ATTACK
	{
		MELEE = 0,
		RANGE
	}
	public ATTACK attack;


	// Use this for initialization
	void Start () 
	{
		attack = PlayerAttackScript.ATTACK.MELEE;
		PlayerMovementScript.isMelee = true;
		StartCoroutine ("att");
		ChargeBarUI.SetActive (false);
		characterstats = gameObject.GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((Input.GetMouseButton (1) || Input.GetButton("X360_B"))){
			attack = PlayerAttackScript.ATTACK.RANGE;
		} else if (Input.GetMouseButton (0) || Input.GetButton("X360_X")) {
			attack = PlayerAttackScript.ATTACK.MELEE;
		}
		
		if (RangeAttRate > 0.0f) {
			RangeAttRate -= Time.fixedDeltaTime;
		}
		if (MeleeAttRate > 0.0f) {
			MeleeAttRate -= Time.fixedDeltaTime;
		}
		if (MeleeAttRate <= 0.0f || RangeAttRate <= 0.0f) {
			isAttack = false;
		}
		if (AttackComboCancelTimer > 0.0f) {
			AttackComboCancelTimer -= Time.fixedDeltaTime;
		} else if (AttackComboCancelTimer <= ACCDuration) {
			numberofattack = 1;
		}

		if (Input.GetKey (KeyCode.C) || Input.GetMouseButton (1) || Input.GetButton("X360_B") || Input.GetButton("X360_X") ) {
			ChargeDuration += Time.fixedDeltaTime;
		}
		if (ChargeDuration > 0.3f) {
			ChargeBarUI.SetActive (true);
		}else 
			ChargeBarUI.SetActive (false);

		if (Input.GetButton ("X360_B") && ChargeDuration > 0.3f) {
			XboxPlayerControlScript.isStartCharged = true;
		} else
			XboxPlayerControlScript.isStartCharged = false;

		if (spinAttack) {
			isAttack = true;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, angle, 0), 10.0f);
			angle+= 10.0f;
			SpinDuration -= Time.fixedDeltaTime;
			if (SpinDuration <= 0.0f) {
				spinAttack = false;
				isAttack = false;
				SpinDuration = 0.0f;
				Destroy (temp_SpinAtt);
				temp_SpinAtt = null;
			}
		}
	}
		
	IEnumerator att()
	{
		while (Alive)
		{
			switch (attack) 
			{
			case ATTACK.MELEE:
				MeleeAtt ();
				break;
			case ATTACK.RANGE:
				Range ();
				break;
			}
			yield return null;
		}
	}

	void MeleeAtt()
	{
		GameObject Temporary_Melee_Handler = null;
		if (Input.GetKeyUp (KeyCode.C)  || Input.GetButtonUp("X360_X") && numberofattack == 1 && !isAttack && PlayerControlScript.isRoll == false && !TextManagerScript.conversationStarted && ChargeDuration <= 0.3f) {
			ChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				numberofattack = 2;
				Temporary_Melee_Handler = Instantiate (Melee, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
				Temporary_Melee_Handler.transform.parent = transform;
				MeleeAttRate = characterstats.MeleeAttRate;
				isAttack = true;
				AttackComboCancelTimer = ACCDuration;

			}
			Destroy (Temporary_Melee_Handler, 2.0f);
		} else if (Input.GetKeyUp (KeyCode.C)  || Input.GetButtonUp("X360_X") && numberofattack == 2 && !isAttack && XboxPlayerControlScript.isRoll == false && ChargeDuration < 0.3f) {
			ChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				numberofattack = 1;
				Temporary_Melee_Handler = Instantiate (Melee2, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
				Temporary_Melee_Handler.transform.parent = transform;
				MeleeAttRate = characterstats.MeleeAttRate;
				isAttack = true;
				AttackComboCancelTimer = ACCDuration;
			} 
			Destroy (Temporary_Melee_Handler, 2.0f);
		} else if ((Input.GetKeyUp (KeyCode.C) || Input.GetButtonUp("X360_X")) && ChargeDuration > 0.3f && !isAttack && XboxPlayerControlScript.isRoll == false) {
			SpinDuration = ChargeDuration;
			if (SpinDuration > 3.0f) {
				SpinDuration = 3.0f;
			}
			ChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				ChargeMeleeAttack (SpinDuration);
			}
		}
	}

	void ChargeMeleeAttack(float SpinDuration)
	{
		//temp_SpinAtt = Instantiate (SpinAttack, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
		//temp_SpinAtt.transform.parent = transform;
		spinAttack = true;	
		MeleeAttRate = characterstats.MeleeAttRate;
		isAttack = true;
		GameObject[] MeleeEnemy = GameObject.FindGameObjectsWithTag ("MeleeEnemy");
		foreach (GameObject n in MeleeEnemy) {
			Destroy (n);
			characterstats.movementSpeed = 5.0f;
		}

	}

	void Range()
	{
		GameObject Temporary_Bullet_Handler = null;
		if (Input.GetMouseButtonUp (1) || Input.GetButtonUp("X360_B") && XboxPlayerControlScript.isRoll == false && !TextManagerScript.conversationStarted && ChargeDuration < 0.3f && !isAttack) {
			if (RangeAttRate <= 0.0f) {
				Temporary_Bullet_Handler = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

				Rigidbody Temporary_RigidBody;
				Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody> ();

				Temporary_RigidBody.AddForce (transform.forward * Bullet_Forward_Force);
				RangeAttRate = characterstats.RangeAttRate;
				ChargeDuration = 0.0f;
			}
			Destroy (Temporary_Bullet_Handler, 3.0f);
		}
		if (Input.GetMouseButtonUp (1) || Input.GetButtonUp("X360_B") && ChargeDuration > 0.3f && XboxPlayerControlScript.isRoll == false && !isAttack) {
			if (ChargeDuration > 3.0f) {
				ChargeDuration = 3.0f;
			}
			ChargeRangeAttack (ChargeDuration);
			ChargeDuration = 0.0f;
		}
	}

	void ChargeRangeAttack(float ChargeDuration)
	{
		temp_bullet = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
		Rigidbody Temp_rigid;
		Temp_rigid = temp_bullet.GetComponent<Rigidbody> ();
		Temp_rigid.AddForce (transform.forward * Bullet_Forward_Force*(1+(ChargeDuration/3.0f)));
		RangeAttRate = characterstats.RangeAttRate;
		Destroy (temp_bullet, 3.0f);
	}
}
