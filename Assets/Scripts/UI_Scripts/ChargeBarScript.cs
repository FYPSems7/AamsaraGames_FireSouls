using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarScript : MonoBehaviour {

	public Image ChargeBar;
	public PlayerAttackScript playerAttackScript;
	public float chargeDuration;
	private float MaxChargeDuration = 3.0f;
	public float ratio;

	// Use this for initialization
	void Start () {
		playerAttackScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttackScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (chargeDuration >= 3.0f) {
			chargeDuration = 3.0f;
		}
	}

	void LateUpdate()
	{
		ChargeBarUpdate ();
	}

	private void ChargeBarUpdate()
	{
		ratio = (float)chargeDuration / (float)MaxChargeDuration;
		ChargeBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
	}
}
