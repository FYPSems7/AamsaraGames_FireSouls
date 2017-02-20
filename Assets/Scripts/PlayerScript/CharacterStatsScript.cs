using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStatsScript : MonoBehaviour {

	public float PlayerHealth = 200.0f;
	public float HealthRegen = 0.0f;
	public static float DivineArmor = 0.0f;
	public static float maxDivineArmor = 100.0f;
	public static float DivineRegen = 10.0f;
	public float MeleeAttack = 100.0f;
	public float MeleeAttRate = 0.75f;
	public float RangeAttack = 60.0f;
	public float RangeAttRate = 0.35f;
	public float movementSpeed = 5.0f;
	public float DodgeSpeed = 0.8f;
	public static float DodgeTimer = 0.25f;
	public static float inviDuration = 0.15f;
	public static bool deityModeEnable = false;
	public static float DeityMode = 100.0f;
	public static bool isDmgRecieved = false;
	public static bool isAlive = true;
	public static float FireSoul;

	float regenTimer = 0.0f;
	float regenDuration = 1.0f;
	float dregenTimer = 0.0f;
	float dregenDuration = 1.0f;
	public float divineCD = 3.0f;
	public float deityTimer = 0.0f;
	float deityDuration = 0.5f;


	float tempHpRegen;
	float tempMAtt;
	float tempMAttRate;
	float tempRangeAtt;
	float tempRangeAttRate;

	public GameObject DeityParticle;
	public GameObject Canvas;
	public GameObject DmgTextPrefab;


	public static int Itemnumbers;
	public int Realitemnumbers;

	void Start()
	{
		DeityParticle.SetActive (false);

		tempHpRegen = HealthRegen;
		tempMAtt = MeleeAttack;
		tempMAttRate = MeleeAttRate;
		tempRangeAtt = RangeAttack;
		tempRangeAttRate = RangeAttRate;
	}

	void Update()
	{
		/*DeityModeFunction ();
		HpRegen ();
		if (DivineArmor > 0) {
			DivineArmorRegen ();
		}*/

		Realitemnumbers = Itemnumbers;

	

		if (movementSpeed <= 0) 
		{
			movementSpeed = 0;
		}
	}

	public void DmgTaken(float Dmg)
	{
		PlayerHealth -= Dmg;
		Vector3 playerPos = new Vector3 (transform.position.x, transform.position.y + 2.2f, transform.position.z);
		Vector3 pos = Camera.main.WorldToScreenPoint (playerPos);

		GameObject DmgText = (GameObject)Instantiate (DmgTextPrefab, pos, Quaternion.identity);
		DmgText.transform.SetParent (Canvas.transform);
		DmgTextPrefab.GetComponent<Text>().text = Dmg.ToString ();
		Destroy (DmgText, 1.0f);

	}

	void HpRegen()
	{
		if (!isDmgRecieved) {
			regenTimer += Time.deltaTime;
			if (regenTimer >= regenDuration) {
				PlayerHealth += HealthRegen;
				regenTimer = 0.0f;
			}
		} else if (isDmgRecieved) {
			divineCD -= Time.deltaTime;
			if (divineCD <= 0.0f) {
				divineCD = 3.0f;
				isDmgRecieved = false;
			}
		}
		if (PlayerHealth > 200.0f) {
			PlayerHealth = 200.0f;
		}
	}

	void DivineArmorRegen()
	{
		if (!isDmgRecieved) {
			dregenTimer += Time.deltaTime;
			if (dregenTimer >= dregenDuration) {
				DivineArmor += DivineRegen;
				dregenTimer = 0.0f;
			}
		} else if (isDmgRecieved) {
			divineCD -= Time.deltaTime;
			if (divineCD <= 0.0f) {
				divineCD = 3.0f;
				isDmgRecieved = false;
			}
		}
		if (DivineArmor >= maxDivineArmor) {
			DivineArmor = maxDivineArmor;
		}
	}

	void DeityModeFunction()
	{
		if (DeityMode >= 100) {
			DeityMode = 100;
			if (Input.GetKeyDown (KeyCode.Q)) {
				deityModeEnable = true;

			}
		}
		if (deityModeEnable) {
			isDmgRecieved = false;
			deityTimer += Time.fixedDeltaTime;
			if (deityTimer >= deityDuration) {
				DeityMode -= 5;
				deityTimer = 0.0f;
			}
			if (DeityMode <= 0) {
				deityModeEnable = false;
			}
			DeityParticle.SetActive (true);
			HealthRegen = 25.0f;
			MeleeAttack = 100.0f * 2.0f;
			MeleeAttRate = 0.75f / 2.0f;
			RangeAttack = 60.0f * 2.0f;
			RangeAttRate = 0.35f / 2.0f;
		}
		if (!deityModeEnable) {
			DeityParticle.SetActive (false);
			HealthRegen = tempHpRegen;
			MeleeAttack = tempMAtt;
			MeleeAttRate = tempMAttRate;
			RangeAttack = tempRangeAtt;
			RangeAttRate = tempRangeAttRate;
		}
	}
}
