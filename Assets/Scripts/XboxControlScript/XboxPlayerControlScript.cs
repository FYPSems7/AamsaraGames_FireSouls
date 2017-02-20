using UnityEngine;
using System.Collections;

public class XboxPlayerControlScript : MonoBehaviour {

	public GameObject Player;
	public WallColliderScript wallcolliderscript;
	public CharacterStatsScript characterstats;
	public PlayerAttackScript playerAttack;
	public Animator anim;
	public AnimatorStateInfo BS;
	static int Run = Animator.StringToHash ("Base Layer.Run");
	static int Idle = Animator.StringToHash ("Base Layer.Idle");
	static int Roll = Animator.StringToHash("Base Layer.Roll");
	static int StrafeLeft = Animator.StringToHash("Base Layer.StrafeLeft");
	static int StrafeRight = Animator.StringToHash("Base Layer.StrafeRight");
	static int StrafeBack = Animator.StringToHash("Base Layer.StrafeBack");
	static int Attack = Animator.StringToHash("Base Layer.Attack");
	static int Attack1 = Animator.StringToHash ("UpperBodyLayer.Attack");


	bool isDodging = false;
	public static bool isRoll = false;
	public static bool isStartCharged = false;


	public float rolltimer = 0.0f;
	float rollduration = 0.5f;
	Vector3 position;
	Vector3 rotation;


	public bool isMovement = false;
	bool LockDirection = false;
	float hAxis;
	float vAxis;





	// Use this for initialization
	void Start () 
	{
		characterstats = gameObject.GetComponent<CharacterStatsScript> ();
		playerAttack = gameObject.GetComponent<PlayerAttackScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		anim.SetBool ("Attack", false);
		hAxis = Input.GetAxis ("Horizontal");
		vAxis = Input.GetAxis ("Vertical");
		if(isStartCharged || isRoll)
		{
			isMovement = false;
			if (!LockDirection || !playerAttack.SpinAttack) {
				transform.forward = Vector3.Normalize (new Vector3 (hAxis, 0, vAxis));

			}
			else
				anim.SetBool ("Run", false);
		}	
		else
		{
			isMovement = true;
		}	

		if(isMovement)
		{
		Movement ();
		}
		if (Input.GetButtonDown ("X360_A") && wallcolliderscript.ishitwall == false && !isRoll) 
		{
			Debug.Log (wallcolliderscript.ishitwall);
			isMovement = false;
			isRoll = true;
		} 
		anim.SetLayerWeight (0, 1f);
		//position = transform.position;
		if (isRoll) 
		{
			Dodge ();
		}

		if (isDodging == true) {
			rolltimer += Time.fixedDeltaTime;
		}
			
		if (Input.GetButtonDown ("X360_X") && !isRoll && !playerAttack.isAttack) {
			anim.SetLayerWeight (1, 1f);
			if (PlayerAttackScript.numberofattack == 1)
				anim.SetInteger ("CurrentAction", 1);
			else if (PlayerAttackScript.numberofattack == 2)
				anim.SetInteger ("CurrentAction", 2);
		} else if (Input.GetButtonUp ("X360_X")) {
			anim.SetInteger ("CurrentAction", 0);
		}

	}


	void Movement()
	{
		if (hAxis!= 0 || vAxis!= 0)  {
			anim.SetBool ("Run", true);
			transform.forward = Vector3.Normalize(new Vector3(hAxis, 0,vAxis));
			transform.Translate (Vector3.forward * Time.deltaTime* characterstats.movementSpeed);
		}else 
			anim.SetBool ("Run", false);
	}

	void Dodge()
	{
		LockDirection = true;
		anim.SetBool ("Roll", true);
		Player.GetComponent<CapsuleCollider> ().enabled = true;
		transform.gameObject.GetComponent<BoxCollider> ().enabled = false;
		isDodging = true;

		transform.Translate (Vector3.forward * Time.deltaTime* characterstats.movementSpeed * characterstats.DodgeSpeed );
		if (rolltimer >= rollduration) {
			LockDirection = false;
			isDodging = false;
			isRoll = false;
			rolltimer = 0.0f;
			anim.SetBool("Roll",false);
			//anim.SetBool ("Idle", true);
			transform.gameObject.GetComponent<BoxCollider> ().enabled = true;
			Player.GetComponent<CapsuleCollider> ().enabled = false;
		}
	}
}
