using UnityEngine;
using System.Collections;

public class PlayerAttackScript : MonoBehaviour {

	public XboxPlayerControlScript xboxScript;
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
	public float MeleeChargeTimer = 3.0f;
	public float MeleeChargeDuration = 0.0f;
	public float RangeChargeDuration = 0.0f;
	public float RangeChargeTimer = 3.0f;
	public float SpinDuration = 0.0f;
	public float TimeTakenToStartCharge = 0.3f;
	float angle = 0.0f;
	public static bool spinAttack = false;
	GameObject MeleeEnemy;

	public float AttackComboCancelTimer = 0.0f;
	float ACCDuration = 1.5f;

	bool Alive = true;
	public bool isAttack = false;
	public bool isCharging = false;
	public bool isMelee = false;
	public bool isRange = false;

	public Animator anim;
	public AnimatorStateInfo BS;
	public int Shoot = Animator.StringToHash ("Base Layer.Shoot");
	public int Attack1 = Animator.StringToHash ("UpperBodyLayer.Attack1");
	public int Attack2 = Animator.StringToHash ("UpperBodyLayer.Attack2");
	public int Attack3 = Animator.StringToHash ("UpperBodyLayer.Attack3");

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
		//-ChargeBarUI.SetActive (false);
		characterstats = gameObject.GetComponent<CharacterStatsScript> ();
		xboxScript = GetComponent<XboxPlayerControlScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		BS = anim.GetCurrentAnimatorStateInfo (0);
		//anim.SetBool ("Aim", false);
		//anim.SetBool ("Shoot", false);
		if (BS.fullPathHash == Shoot) 
		{
			anim.SetBool ("Shoot", false);
		}
		if (Input.GetButton("X360_B") && !isAttack && !spinAttack)
		{
			attack = PlayerAttackScript.ATTACK.RANGE;
			anim.SetBool ("Aim", true);
		} 
		else if (Input.GetButton("X360_X"))
		{
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

		if (Input.GetButton ("X360_X") && !isAttack && !spinAttack && !XboxPlayerControlScript.isRoll && !isRange)
		{
			isMelee = true;
			MeleeChargeDuration += Time.fixedDeltaTime;
			if (MeleeChargeDuration > 3.0f) {
				MeleeChargeDuration = 3.0f;
			}
		} 
		if (Input.GetButton ("X360_B") && !isAttack && !spinAttack && !XboxPlayerControlScript.isRoll && !isMelee) 
		{
			isRange = true;
			RangeChargeDuration += Time.fixedDeltaTime;
			if (RangeChargeDuration > 3.0f) 
			{
				RangeChargeDuration = 3.0f;
			}
		}
		if (MeleeChargeDuration > TimeTakenToStartCharge) {
			isCharging = true;
			ChargeBarUI.GetComponent<ChargeBarScript> ().chargeDuration = MeleeChargeDuration;
			//ChargeBarUI.SetActive (true);
		}
		if(RangeChargeDuration > TimeTakenToStartCharge)
		{
			isCharging = true;
			ChargeBarUI.GetComponent<ChargeBarScript> ().chargeDuration = RangeChargeDuration;
		}else {
			ChargeBarUI.GetComponent<ChargeBarScript> ().ratio = 0.0f;
			isCharging = false;
			//ChargeBarUI.SetActive (false);
		}

		if (Input.GetButton ("X360_B") && RangeChargeDuration > TimeTakenToStartCharge) {
			XboxPlayerControlScript.isStartCharged = true;
			anim.SetBool ("Aim", true);
		} else {
			XboxPlayerControlScript.isStartCharged = false;
			anim.SetBool ("Aim", false);
		}
		if (spinAttack) 
		{
			//anim.SetBool("Spin",true);
			//anim.SetBool ("Run", false);
			isAttack = true;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, angle, 0), 10.0f);
			angle+= 10.0f;
			SpinDuration -= Time.fixedDeltaTime;
			if (SpinDuration <= 0.0f) {
				isMelee = false;
				spinAttack = false;
				isAttack = false;
				SpinDuration = 0.0f;
				Destroy (temp_SpinAtt);
				temp_SpinAtt = null;
				anim.SetBool("Spin",false);
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
		if (Input.GetButtonUp ("X360_X") && numberofattack == 1 && !isAttack && PlayerControlScript.isRoll == false && !TextManagerScript.conversationStarted && MeleeChargeDuration < TimeTakenToStartCharge) {
			MeleeChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				numberofattack = 2;
				Temporary_Melee_Handler = Instantiate (Melee, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
				Temporary_Melee_Handler.transform.parent = transform;
				MeleeAttRate = characterstats.MeleeAttRate;
				isAttack = true;
				AttackComboCancelTimer = ACCDuration;
				isMelee = false;
			}
			Destroy (Temporary_Melee_Handler, 2.0f);
		} else if (Input.GetButtonUp ("X360_X") && numberofattack == 2 && !isAttack && XboxPlayerControlScript.isRoll == false && MeleeChargeDuration < TimeTakenToStartCharge) {
			MeleeChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				numberofattack = 3;
				Temporary_Melee_Handler = Instantiate (Melee2, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
				Temporary_Melee_Handler.transform.parent = transform;
				MeleeAttRate = characterstats.MeleeAttRate;
				isAttack = true;
				AttackComboCancelTimer = ACCDuration;
				isMelee = false;
			} 
			Destroy (Temporary_Melee_Handler, 2.0f);
		} else if (Input.GetButtonUp ("X360_X") && numberofattack == 3 && !isAttack && XboxPlayerControlScript.isRoll == false && MeleeChargeDuration < TimeTakenToStartCharge) {
			MeleeChargeDuration = 0.0f;
			if (MeleeAttRate <= 0.0f) {
				Debug.Log ("Run");
				isMelee = false;
				numberofattack = 1;
				Temporary_Melee_Handler = Instantiate (Melee, Melee_Emitter.transform.position, Melee_Emitter.transform.rotation) as GameObject;
				Temporary_Melee_Handler.transform.parent = transform;
				MeleeAttRate = characterstats.MeleeAttRate;
				isAttack = true;
				AttackComboCancelTimer = ACCDuration;
			} 
			Destroy (Temporary_Melee_Handler, 2.0f);
		} else if (Input.GetButtonUp ("X360_X") && MeleeChargeDuration > TimeTakenToStartCharge && !isAttack && XboxPlayerControlScript.isRoll == false) {
			SpinDuration = MeleeChargeDuration;
			if (SpinDuration > 3.0f) {
				SpinDuration = 3.0f;
			}
			MeleeChargeDuration = 0.0f;
			ChargeBarUI.GetComponent<ChargeBarScript> ().chargeDuration = MeleeChargeDuration;
			if (MeleeAttRate <= 0.0f) {
				ChargeMeleeAttack (SpinDuration);

				anim.SetBool ("Spin", true);
				anim.SetBool ("Run", false);
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
		foreach (GameObject n in MeleeEnemy) 
		{
			Destroy (n);
			characterstats.movementSpeed = 5.0f;
		}


	}

	void Range()
	{
		GameObject Temporary_Bullet_Handler = null;
		if (Input.GetButtonUp("X360_B") && XboxPlayerControlScript.isRoll == false && !TextManagerScript.conversationStarted && RangeChargeDuration < TimeTakenToStartCharge && !isAttack && !spinAttack) {
			anim.SetBool ("Aim", false);
			anim.SetBool ("Shoot", true);
			if (RangeAttRate <= 0.0f) 
			{
				isRange = false;
				Temporary_Bullet_Handler = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
				Rigidbody Temporary_RigidBody;
				Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody> ();

				Temporary_RigidBody.AddForce (transform.forward * Bullet_Forward_Force);
				RangeAttRate = characterstats.RangeAttRate;
				RangeChargeDuration = 0.0f;

			} 
			Destroy (Temporary_Bullet_Handler, 3.0f);
		}

		if (Input.GetButtonUp("X360_B") && RangeChargeDuration > TimeTakenToStartCharge && XboxPlayerControlScript.isRoll == false && !isAttack && !spinAttack) {
			isRange = false;
			anim.SetBool ("Aim", false);
			anim.SetBool ("Shoot", true);
			ChargeRangeAttack (RangeChargeDuration);
			RangeChargeDuration = 0.0f;
			ChargeBarUI.GetComponent<ChargeBarScript> ().chargeDuration = RangeChargeDuration;
			anim.SetBool ("Run", false);

		}
	}

	void ChargeRangeAttack(float ChargeDuration)
	{
		
		temp_bullet = Instantiate (Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
		Rigidbody Temp_rigid;
		Temp_rigid = temp_bullet.GetComponent<Rigidbody> ();
		Temp_rigid.AddForce (transform.forward * Bullet_Forward_Force*(1+(ChargeDuration/3.0f)));
		RangeAttRate = characterstats.RangeAttRate;
		isRange = false;
		Destroy (temp_bullet, 3.0f);
	}
}
