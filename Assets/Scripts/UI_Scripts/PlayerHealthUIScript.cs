using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIScript : MonoBehaviour {

	public CharacterStatsScript characterstats;
	public Image PlayerHP_Front;
	public Text FrontText;
	public Text BackgroundText;

	private int CurrentPlayerHealth;
	private int MaxPlayerHealth = 200;

	// Use this for initialization
	void Start () {
		characterstats = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		CurrentPlayerHealth = (int)characterstats.PlayerHealth;

	}

	void LateUpdate()
	{
		HealthBarUpdate ();
	}

	private void HealthBarUpdate()
	{
		float ratio = (float)CurrentPlayerHealth / (float)MaxPlayerHealth;
		PlayerHP_Front.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		FrontText.text = CurrentPlayerHealth.ToString () + " / " + MaxPlayerHealth.ToString ();
		BackgroundText.text = CurrentPlayerHealth.ToString () + " / " + MaxPlayerHealth.ToString ();

	}
}
