using UnityEngine;
using System.Collections;

public class LevelManagerScript : MonoBehaviour {

	public GameObject currentCheckpoint;
	public CharacterStatsScript characterstats;
	private PlayerMovementScript player;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerMovementScript> ();
		characterstats = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (characterstats.PlayerHealth <= 0) 
		{
			RespawnPlayer ();
			characterstats.PlayerHealth = 200.0f;
		}

	}

	public void RespawnPlayer()
	{
		player.transform.position = currentCheckpoint.transform.position;
	}
}
