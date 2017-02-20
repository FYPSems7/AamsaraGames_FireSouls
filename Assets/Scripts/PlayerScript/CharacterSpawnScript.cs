using UnityEngine;
using System.Collections;

public class CharacterSpawnScript : MonoBehaviour {

	public GameObject spawnpoint;
	public CharacterStatsScript characterstats;
	public bool respawn = false;


	// Use this for initialization
	void Start () 
	{
		characterstats = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (characterstats.PlayerHealth <= 0) 
		{
			respawn = true;
		} else 
		{
			respawn = false;
		}



		if (respawn) 
		{
			characterstats.PlayerHealth = 200.0f;
			transform.position = spawnpoint.transform.position;
		}
	}


}
