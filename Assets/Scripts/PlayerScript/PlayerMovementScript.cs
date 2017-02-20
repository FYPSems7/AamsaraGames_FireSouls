using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

	public GameObject Player;
	public EnemyLockOnScript enemyLockOnScript;
	public WallColliderScript wallcolliderscript;
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

	public bool isDodging = false;

	public bool up = false;
	public bool down = false;
	public bool left = false;
	public bool right = false;
	public bool roll = false;

	//float invi = 0.0f;
	public float rolltimer = 0.0f;
	public float rolltimerduration = 0.7f;
	public float animSpeed = 1.0f;

	public bool ismovementEnabled = false;
	public static bool isMelee = true;
	Vector3 LookPos;

	public int numberofattack = 0;

	// Use this for initialization
	void Start ()
	{
		Physics.gravity = new Vector3(0, -200.0f, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		anim.SetLayerWeight (0, 1f);
		BS = anim.GetCurrentAnimatorStateInfo (0);
		if (CharacterStatsScript.deityModeEnable) {
			animSpeed = 1 / Time.timeScale;
			anim.speed = animSpeed;
		} else
			anim.speed = animSpeed;
		if (roll) {
			if (CharacterStatsScript.deityModeEnable) {
				anim.speed = animSpeed * 2.0f;
			} else
				anim.speed = 2.0f;
		} else if (!roll) {
			anim.speed = 1.0f;
		}
		if (EnemyLockOnScript.isLockOn == true) 
		{
			Vector3 tempEnemy = new Vector3(enemyLockOnScript.GetComponent<EnemyLockOnScript>().EnemyLockedOn.transform.position.x, 0.0f, enemyLockOnScript.GetComponent<EnemyLockOnScript>().EnemyLockedOn.transform.position.z);
			//Vector3 tempEnemy = enemyLockOnScript.GetComponent<EnemyLockOnScript>().EnemyLockedOn.transform.position;
			transform.LookAt(tempEnemy);
			anim.SetBool ("Run", false);
			anim.SetBool ("SLeft", false);
			anim.SetBool("Roll",false);

			/*
			if (Input.GetKey (KeyCode.A)) 
			{
				transform.Translate ((Vector3.left) * 3.0f * Time.deltaTime);
				anim.SetBool ("SLeft", true);
			}

			anim.SetBool ("SRight", false);
			if (Input.GetKey (KeyCode.D)) 
			{
				transform.Translate ((Vector3.right) * 3.0f * Time.deltaTime);
				anim.SetBool ("SRight", true);
			}

			anim.SetBool ("SBack", false);
			if (Input.GetKey (KeyCode.S)) 
			{
				transform.Translate ((Vector3.back) * 3.0f * Time.deltaTime);
				anim.SetBool ("SBack", true);
			}
			anim.SetBool("Run", false);
			if(Input.GetKey(KeyCode.W))
			{
				transform.Translate((Vector3.forward)* 3.0f * Time.deltaTime);
				anim.SetBool("Run", true);
			}
			*/

		}

		if (EnemyLockOnScript.isLockOn == false && isMelee == true) {
			anim.SetBool ("Run", false);

			anim.SetBool ("Attack", false);

			//anim.SetBool ("Roll", false);

			if (Input.GetMouseButtonDown (0) && isDodging == false && roll == false) 
			{
				anim.SetLayerWeight (1, 1f);
				numberofattack++;
				if(numberofattack == 1)
					anim.SetInteger ("CurrentAction", 1);
				else if (numberofattack == 2)
					anim.SetInteger ("CurrentAction", 2);

			} 
			else if (Input.GetMouseButtonUp (0)) 
			{
				anim.SetInteger ("CurrentAction", 0);
				if (numberofattack >= 2)
					numberofattack = 0;

			}


			if (roll) {
				Dodge ();
			}


			if (Input.GetKey (KeyCode.Space) && wallcolliderscript.ishitwall == false) {
				roll = true;

				anim.SetBool ("Roll", true);
			}

			if (Input.GetKey (KeyCode.W)) {
				transform.Translate ((Vector3.forward) * 5.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 0, 0);
				up = true;
				down = false;
				left = false;
				right = false;
				anim.SetBool ("Run", true);



			} else if (Input.GetKey (KeyCode.S)) {
				transform.Translate ((Vector3.forward) * 5.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 180, 0);
				up = false;
				down = true;
				left = false;
				right = false;
				anim.SetBool ("Run", true);


			} else if (Input.GetKey (KeyCode.A)) {
				transform.Translate ((Vector3.forward) * 5.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 270, 0);
				up = false;
				down = false;
				left = true;
				right = false;
				anim.SetBool ("Run", true);


			} else if (Input.GetKey (KeyCode.D)) {
				transform.Translate ((Vector3.forward) * 5.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 90, 0);
				anim.SetBool ("Run", true);
				up = false;
				down = false;
				left = false;
				right = true;

			}

			if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.W)) {
				transform.Translate ((Vector3.forward) * 3.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 45, 0);
				anim.SetBool ("Run", true);


			}

			if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.W)) {
				transform.Translate ((Vector3.forward) * 3.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 315, 0);
				anim.SetBool ("Run", true);


			}

			if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.S)) {
				transform.Translate ((Vector3.forward) * 3.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 135, 0);
				anim.SetBool ("Run", true);


			}

			if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.S)) {
				transform.Translate ((Vector3.forward) * 3.0f * Time.fixedDeltaTime);
				transform.rotation = Quaternion.Euler (0, 225, 0);
				anim.SetBool ("Run", true);

			}
		} else if (!isMelee) {
			RangeMode ();
		}
	}


	void Dodge()
	{
		Player.GetComponent<CapsuleCollider> ().enabled = true;
		transform.gameObject.GetComponent<BoxCollider> ().enabled = false;
		isDodging = true;
		rolltimer += Time.fixedDeltaTime;
		if(up)
			transform.Translate ((Vector3.forward) * 0.5f * Time.fixedDeltaTime, Space.World);
		else if(down)
			transform.Translate ((Vector3.back) * 0.5f * Time.fixedDeltaTime, Space.World);
		else if(left)
			transform.Translate ((Vector3.left) * 0.5f * Time.fixedDeltaTime, Space.World);
		else if(right)
			transform.Translate ((Vector3.right) * 0.5f * Time.fixedDeltaTime, Space.World);
		if(rolltimer > rolltimerduration)
		{
			transform.gameObject.GetComponent<BoxCollider> ().enabled = true;
			anim.SetBool("Roll",false);
			anim.SetBool ("Idle", true);
			anim.SetLayerWeight (1, 1f);
			roll = false;
			rolltimer = 0.0f;
			isDodging = false;
			Player.GetComponent<CapsuleCollider> ().enabled = false;
		}

	}


	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		float pushPower = 2.0F;
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;

		if (hit.moveDirection.y < -0.3F)
			return;

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;

	}

	void RangeMode ()
	{
		/*Vector3 position = Input.mousePosition;
		position.z = Mathf.Abs (Camera.main.transform.position.y - transform.position.y);
		position = Camera.main.WorldToScreenPoint (position);
		transform.LookAt (position);*/
		Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayLength;

		anim.SetBool ("Run", false);
		anim.SetBool ("Roll", false);

		if (groundPlane.Raycast (cameraRay, out rayLength)) {
			Vector3 pointToLook = cameraRay.GetPoint (rayLength);
			Debug.DrawLine (cameraRay.origin, pointToLook, Color.blue);
			transform.LookAt (pointToLook);
		}
	}
}
