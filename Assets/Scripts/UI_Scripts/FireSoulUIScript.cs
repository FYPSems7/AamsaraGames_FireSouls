using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSoulUIScript : MonoBehaviour {

	public Text FireSoulText;
	private int FireSoul;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		FireSoul = (int)CharacterStatsScript.FireSoul;
		FireSoulUpdate ();
	}

	private void FireSoulUpdate()
	{
		FireSoulText.text = FireSoul.ToString ();
	}
}
