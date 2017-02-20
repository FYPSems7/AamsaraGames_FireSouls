using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

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

	bool isDodging = false;
	public static bool isRoll = false;
	bool up = false;
	bool down = false;
	bool left = false;
	bool right = false;

	float animSpeed = 1.0f;
	public float rolltimer = 0.0f;
	float rollduration = 0.6f;
	Vector3 position;
	Vector3 rotation;
	float angle = 0.0f;
	float speed = 0.1f;

	public enum Movement
	{
		Up = 0,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight
	}
	public Movement movement;

	IEnumerator I_movement()
	{
		switch (movement) {
		case Movement.Up:
			Up ();
			break;
		case Movement.Down:
			Down ();
			break;
		case Movement.Left:
			Left ();
			break;
		case Movement.Right:
			Right ();
			break;
		case Movement.UpLeft:
			UpLeft ();
			break;
		case Movement.UpRight:
			UpRight ();
			break;
		case Movement.DownLeft:
			DownLeft ();
			break;
		case Movement.DownRight:
			DownRight ();
			break;
		}
		yield return null;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetLayerWeight (0, 1f);
		position = transform.position;
		anim.speed = animSpeed;
		if (isRoll) {
			Dodge ();
		}
		if (isDodging == true) {
			rolltimer += Time.fixedDeltaTime;
		}
		if (EnemyLockOnScript.isLockOn == true) {
			GameObject tempEnemy = enemyLockOnScript.GetComponent<EnemyLockOnScript>().EnemyLockedOn.gameObject;
			if (!isRoll) {
				transform.LookAt (tempEnemy.transform.position);
			}
		}
		KeyInput();
		transform.position = position;
	}

	void KeyInput()
	{
		up = false;
		down = false;
		left = false;
		right = false;
		anim.SetBool ("Attack", false);
		anim.SetBool ("Run", false);
		StopCoroutine ("I_movement");	
		angle = 0.0f;

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A)) {
			movement = Movement.UpLeft;
			up = true;
			left = true;
			angle = 315.0f;
			StartCoroutine ("I_movement");
		} else if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) {
			movement = Movement.UpRight;
			up = true;
			right = true;
			angle = 45.0f;
			StartCoroutine ("I_movement");
		} else if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A)) {
			movement = Movement.DownLeft;
			down = true;
			left = true;
			angle = 225.0f;
			StartCoroutine ("I_movement");
		}else if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) {
			movement = Movement.DownRight;
			down = true;
			right = true;
			angle = 135.0f;
			StartCoroutine ("I_movement");
		}else if (Input.GetKey (KeyCode.W)) {
			movement = Movement.Up;
			up = true;
			angle = 0.0f;
			StartCoroutine ("I_movement");
		} else if (Input.GetKey (KeyCode.S)) {
			movement = Movement.Down;
			down = true;
			angle = 180.0f;
			StartCoroutine ("I_movement");
		} else if (Input.GetKey (KeyCode.A)) {
			movement = Movement.Left;
			left = true;
			angle = 270.0f;
			StartCoroutine ("I_movement");
		} else if (Input.GetKey (KeyCode.D)) {
			movement = Movement.Right;
			right = true;
			angle = 90.0f;
			StartCoroutine ("I_movement");
		}

		if (Input.GetKey (KeyCode.Space) && wallcolliderscript.ishitwall == false && (up||down||left||right) && !isRoll) {
			isRoll = true;
		}

		if (Input.GetMouseButtonDown (0) && !isRoll) {
			anim.SetLayerWeight (1, 1f);
			if (PlayerAttackScript.numberofattack == 1)
				anim.SetInteger ("CurrentAction", 1);
			else if (PlayerAttackScript.numberofattack == 2)
				anim.SetInteger ("CurrentAction", 2);
		} else if (Input.GetMouseButtonUp (0)) {
			anim.SetInteger ("CurrentAction", 0);
		}
	}

	void Up()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 0, 0);
		} 
		position.z += speed;
		//transform.position = position;
		anim.SetBool ("Run", true);
	}
	void Down()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 180, 0);
		}
		position.z -= speed;
		//transform.position = position;
		anim.SetBool ("Run", true);

	}
	void Left()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 270, 0);
		}
		position.x -= speed;
		//transform.position = position;
		anim.SetBool ("Run", true);

	}
	void Right()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 90, 0);
		}
		position.x += speed;
		//transform.position = position;
		anim.SetBool ("Run", true);
	}

	void UpLeft()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 315, 0);
		}
		position.x -= speed*0.75f;
		position.z += speed*0.75f;
		transform.position = position;
		anim.SetBool ("Run", true);
	}
	void UpRight()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 45, 0);
		}
		position.x += speed*0.75f;
		position.z += speed*0.75f;
		transform.position = position;
		anim.SetBool ("Run", true);
	}
	void DownLeft()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 225, 0);
		}
		position.x -= speed*0.75f;
		position.z -= speed*0.75f;
		transform.position = position;
		anim.SetBool ("Run", true);
	}
	void DownRight()
	{
		if (EnemyLockOnScript.isLockOn == false && !PlayerAttackScript.spinAttack) {
			transform.rotation = Quaternion.Euler (0, 135, 0);
		}
		position.x += speed*0.75f;
		position.z -= speed*0.75f;
		transform.position = position;
		anim.SetBool ("Run", true);
	}

	void Dodge()
	{
		animSpeed = 2.0f;
		anim.SetBool ("Roll", true);
		Player.GetComponent<CapsuleCollider> ().enabled = true;
		transform.gameObject.GetComponent<BoxCollider> ().enabled = false;
		isDodging = true;
		if (up) {
			rotation.y = angle;
			position.z += speed * 0.25f;
		} else if (down) {
			rotation.y = angle;
			position.z -= speed * 0.25f;
		} else if (left) {
			rotation.y = angle;
			position.x -= speed * 0.25f;
		} else if (right) {
			rotation.y = angle;
			position.x += speed * 0.25f;
		}
		transform.rotation = Quaternion.Euler(rotation);
		if (rolltimer >= rollduration) {
			animSpeed = 1.0f;
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
