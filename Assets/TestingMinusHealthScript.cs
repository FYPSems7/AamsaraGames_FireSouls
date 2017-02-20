using UnityEngine;
using System.Collections;

public class TestingMinusHealthScript : MonoBehaviour {


	public CharacterStatsScript characterstats;

	// Use this for initialization
	void Start () {
		characterstats = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player") 
		{
			characterstats.PlayerHealth -= 50.0f;

		}
	}
}
